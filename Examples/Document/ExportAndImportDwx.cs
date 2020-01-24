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
        private static void ExportArchivedDocument(Organization organization)
        {
            Console.WriteLine("ExportArchivedDocument");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.dwx");

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = null;

                DialogExpression dialogExpression = new DialogExpression()
                {
                    Operation = DialogExpressionOperation.And,
                    Condition = new List<DialogExpressionCondition>()
                    {
                        DialogExpressionCondition.Create("DWDOCID", documentId.ToString())
                    },
                    Count = 100,
                    SortOrder = new List<SortedField>()
                    {
                        SortedField.Create("DWDOCID", SortDirection.Desc)
                    }
                };

                DialogInfos dialogInfos = fileCabinet.GetDialogInfosFromDialogsRelation();

                if (dialogInfos == null)
                {
                    Console.WriteLine("DialogInfos is null!");
                }
                else
                {
                    DialogInfo dialog = dialogInfos.Dialog.FirstOrDefault(d => d.Id == queryDialogId);

                    if (dialog == null)
                    {
                        Console.WriteLine("Dialog is null!");
                    }
                    else
                    {
                        DocumentsQueryResult documentsQueryResult =
                            dialog.GetDialogFromSelfRelation().GetDocumentsResult(dialogExpression);

                        Console.WriteLine("Query Result");
                        document = documentsQueryResult.Items.FirstOrDefault();

                        document = document?.GetDocumentFromSelfRelation();
                    }
                }

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    document = document.GetDocumentFromSelfRelation();

                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        using (Stream documentStream = document.PostToDownloadAsArchiveRelationForStream(
                            new ExportSettings()
                            {
                                ExportTextshots = true
                            }))
                        {
                            documentStream.CopyTo(fs);
                        }
                    }

                    Console.WriteLine($"Export archive document {filePath} created!");
                }
            }
        }

        private static void ExportArchivedDocuments(Organization organization)
        {
            Console.WriteLine("ExportArchivedDocuments");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string filePath = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid().ToString()}.dwx");

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                DialogInfo dialogInfo = fileCabinet.GetDialogInfosFromDialogsRelation().Dialog.FirstOrDefault(d => d.GetDialogFromSelfRelation().Query != null);

                Dialog dialog = dialogInfo.GetDialogFromSelfRelation();

                DialogExpression dialogExpression = new DialogExpression()
                {
                    Operation = DialogExpressionOperation.And,
                    Condition = new List<DialogExpressionCondition>()
                    {
                        DialogExpressionCondition.Create("DWDOCID", "1")
                    },
                    Count = 100,
                    SortOrder = new List<SortedField>()
                    {
                        SortedField.Create("DWDOCID", SortDirection.Desc)
                    }
                };

                DocumentsQueryResult documentsQueryResult =
                    dialog.Query.PostToDialogExpressionRelationForDocumentsQueryResult(dialogExpression);

                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                {
                    using (Stream documentStream = documentsQueryResult.PostToExportDocumentsRelationForStream(
                        new ExportSettings()
                        {
                            ExportTextshots = true,
                            ExportHistory = true
                        }))
                    {
                        documentStream.CopyTo(fs);
                    }
                }

                Console.WriteLine($"Export archive documents {filePath} created!");
            }
        }

        private static void ImportArchivedDocument(Organization organization)
        {
            Console.WriteLine("ImportArchivedDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string filePath = @"C:\Users\{username}\AppData\Local\Temp\00000000-0000-0000-0000-000000000000.dwx";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                ImportResult importResult = fileCabinet.ImportArchive(new FileInfo(filePath));

                for (int i = 0; i < importResult.Results.Count; i++)
                {
                    int count = i + 1;

                    ImportResultEntry result = importResult.Results[i];

                    switch (result.Status)
                    {
                        case ImportEntryStatus.Succeeded:
                            Console.WriteLine($"Successfully import archive part {count}/{importResult.Results.Count} - {filePath}");
                            break;
                        case ImportEntryStatus.Failed:
                            Console.WriteLine($"Failed import archive {count}/{importResult.Results.Count} - {filePath}\n ErrorMessage: {result.ErrorMessage}");
                            break;
                    }
                }
            }
        }
    }
}
