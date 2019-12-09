using DocuWare.Platform.ServerClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class Dialog
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dialog examples!");

            string serverName = @"";
            string serverAddress = @"https://" + serverName + @"/DocuWare/Platform/";
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
                    UploadDocument(organization);
                }
            }
            Console.Read();
        }

        static void UploadDocument(Organization organization)
        {
            Console.WriteLine("UploadDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string dialogId = "00000000-0000-0000-0000-000000000000";
            string fileInfoPath = @"C:\Temp\Test.pdf";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromStoresRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfo is null!");
                }
                else
                {
                    DialogInfo dialog = dialogInfos.Dialog.FirstOrDefault(d => d.Id == dialogId);

                    if (dialog == null)
                    {
                        Console.WriteLine("Dialog is null!");
                    }
                    else
                    {
                        Document metaDocument = new Document()
                        {
                            Fields = new List<DocumentIndexField>()
                            {
                                //Create index value => field name, value
                                DocumentIndexField.Create("NAME", "TestUpload")
                            }
                        };

                        dialog.EasyUploadDocument(new FileInfo[] { new FileInfo(fileInfoPath) }, metaDocument);
                    }
                }
            }
        }
    }
}
