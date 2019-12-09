using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DocuWare.Platform.ServerClient;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    partial class Document
    {
        static void ReplaceAllSectionsInDocument(Organization organization)
        {
            Console.WriteLine("ReplaceAllSectionsInDocument");

            List<string> newSectionsPath = new List<string> { @"C:\Temp\File1.pdf", @"C:\Temp\File2.pdf" };
            int documentId = 1;

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault();

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation().Items.FirstOrDefault(d => d.Id == documentId)?.GetDocumentFromSelfRelation();

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                { 
                    //Delete all sections
                    document.Sections.ForEach(s => s.DeleteSelfRelation());

                    //Upload new sections
                    foreach (var newSectionPath in newSectionsPath)
                    {
                        document.EasyUploadFile(new FileInfo(newSectionPath));
                    }
                }
            }
        }

        static void ReplaceSpecificSectionInDocument(Organization organization)
        {
            Console.WriteLine("ReplaceSpecificSectionInDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string sectionId = "1-1";
            string newSectionPath = @"C:\Temp\Test.pdf";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation().Items.FirstOrDefault(d => d.Id == documentId)?.GetDocumentFromSelfRelation();

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    //Get specific section(don't forget the self relation) and replace it
                    Section section = document.Sections.FirstOrDefault(s => s.Id == sectionId)?.GetSectionFromSelfRelation();

                    if (section == null)
                    {
                        Console.WriteLine("Section is null!");
                    }
                    else
                    {
                        section.EasyReplaceFile(new FileInfo(newSectionPath));
                    }
                }
            }
        }
    }
}
