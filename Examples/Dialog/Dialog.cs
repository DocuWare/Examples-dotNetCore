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
                Organization organization = authenticator.Organization;
                
                if (organization == null)
                {
                    Console.WriteLine("No organization found");
                }
                else
                {
                    ListAllDialogs(organization);
                    ListAllStoreDialogs(organization);
                    ListAllSearchDialogs(organization);
                    ListAllListDialogs(organization);
                    ListAllTaskDialogs(organization);
                    Query(organization);
                    UploadDocument(organization);
                }
            }
            Console.Read();
        }

        private static void ListAllDialogs(Organization organization)
        {
            Console.WriteLine("ListAllDialogs");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromDialogsRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfo is null!");
                }
                else
                {
                    Console.WriteLine("Dialogs");
                    dialogInfos.Dialog.ForEach(d => Console.WriteLine($"ID: {d.Id} - DisplayName: {d.DisplayName} - Type: {d.Type}"));
                }
            }
        }

        private static void ListAllStoreDialogs(Organization organization)
        {
            Console.WriteLine("ListAllStoreDialogs");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

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
                    Console.WriteLine("Store Dialogs");
                    dialogInfos.Dialog.ForEach(d => Console.WriteLine($"ID: {d.Id} - DisplayName: {d.DisplayName} - Type: {d.Type}"));
                }
            }
        }

        private static void ListAllSearchDialogs(Organization organization)
        {
            Console.WriteLine("ListAllSearchDialogs");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromSearchesRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfo is null!");
                }
                else
                {
                    Console.WriteLine("Search Dialogs");
                    dialogInfos.Dialog.ForEach(d => Console.WriteLine($"ID: {d.Id} - DisplayName: {d.DisplayName} - Type: {d.Type}"));
                }
            }
        }

        private static void ListAllListDialogs(Organization organization)
        {
            Console.WriteLine("ListAllListDialogs");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromResultTreesRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfo is null!");
                }
                else
                {
                    Console.WriteLine("List Dialogs");
                    dialogInfos.Dialog.ForEach(d => Console.WriteLine($"ID: {d.Id} - DisplayName: {d.DisplayName} - Type: {d.Type}"));
                }
            }
        }

        private static void ListAllTaskDialogs(Organization organization)
        {
            Console.WriteLine("ListAllTaskDialogs");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromTaskListsRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfo is null!");
                }
                else
                {
                    Console.WriteLine("Task Dialogs");
                    dialogInfos.Dialog.ForEach(d => Console.WriteLine($"ID: {d.Id} - DisplayName: {d.DisplayName} - Type: {d.Type}"));
                }
            }
        }

        private static void Query(Organization organization)
        {
            Console.WriteLine("Query");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string dialogId = "00000000-0000-0000-0000-000000000000";

            DialogExpression dialogExpression = new DialogExpression()
            {
                Operation = DialogExpressionOperation.And,
                Condition = new List<DialogExpressionCondition>()
                {
                    DialogExpressionCondition.Create("NAME", "T*")
                },
                Count = 100,
                SortOrder = new List<SortedField>()
                {
                    SortedField.Create("NAME", SortDirection.Desc)
                }
            };

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromDialogsRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfos is null!");
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
                        DocumentsQueryResult documentsQueryResult = dialog.GetDialogFromSelfRelation().GetDocumentsResult(dialogExpression);

                        Console.WriteLine("Query Result");
                        foreach (Document document in documentsQueryResult.Items)
                        {
                            Console.WriteLine($"ID {document.Id}");
                            Console.WriteLine("Fields");
                            document.Fields.ForEach(f => Console.WriteLine($"Name: {f.FieldName} - Item: {f.Item}"));
                        }
                    }
                }
            }
        }

        private static void UploadDocument(Organization organization)
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
                            Title = "My Test Document",
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
