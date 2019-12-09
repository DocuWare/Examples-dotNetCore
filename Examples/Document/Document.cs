using System;
using System.Collections.Generic;
using System.Linq;
using DocuWare.Platform.ServerClient;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    partial class Document
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Document Examples!");

            string serverName = @"";
            string serverAddress = @"http://" + serverName + @"/DocuWare/Platform/";
            string userName = "";
            string userPassword = "";

            using (Helpers.Authenticator authenticator = new Helpers.Authenticator(serverAddress, userName, userPassword))
            {
                Organization organization = authenticator.ServiceConnection.Organizations.FirstOrDefault();
                if (organization == null)
                {
                    Console.WriteLine("No organization found");
                }
                else
                {
                    MergeDocuments(organization);
                    DivideDocuments(organization);
                    ReplaceAllSectionsInDocument(organization);
                    ReplaceSpecificSectionInDocument(organization);
                }
            }
            Console.Read();
        }

        private static void MergeDocuments(Organization organization)
        {
            Console.WriteLine("MergeDocuments");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            List<int> documentIds = new List<int>() { 1, 2 };

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                ContentMergeOperationInfo contentMergeOperationInfo = new ContentMergeOperationInfo()
                {
                    Documents = documentIds,
                    Operation = ContentMergeOperation.Staple,
                    Force = true
                };

                Platform.ServerClient.Document mergedDocument = fileCabinet.PutToContentMergeOperationRelationForDocument(contentMergeOperationInfo);
            }
        }

        private static void DivideDocuments(Organization organization)
        {
            Console.WriteLine("DivideDocuments");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation().Items.FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    ContentDivideOperationInfo contentDivideOperationInfo = new ContentDivideOperationInfo()
                    {
                        Operation = ContentDivideOperation.Unstaple,
                        Force = true
                    };

                    DocumentsQueryResult dividedDocuments = document.PutToContentDivideOperationRelationForDocumentsQueryResult(contentDivideOperationInfo);
                }
            }
        }
    }
}
