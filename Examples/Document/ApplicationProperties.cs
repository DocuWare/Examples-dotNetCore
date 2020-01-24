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
        private static void CreateDocumentApplicationProperties(Organization organization)
        {
            Console.WriteLine("CreateDocumentApplicationProperties");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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

                    DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                    documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                    documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key1", Value = "Test" });
                    documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key2", Value = "Delete" });

                    DocumentApplicationProperties resultDocumentApplicationProperties = document.PostToAppPropertiesRelationForDocumentApplicationProperties(documentApplicationProperties);
                }
            }
        }

        private static void CreateDocumentApplicationPropertiesWithNewDocument(Organization organization)
        {
            Console.WriteLine("CreateDocumentApplicationPropertiesWithNewDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document metaDocument = new Platform.ServerClient.Document();

                metaDocument.ApplicationProperties = new List<DocumentApplicationProperty>();

                metaDocument.ApplicationProperties.Add(new DocumentApplicationProperty() { Name = "key1", Value = "Test" });
                metaDocument.ApplicationProperties.Add(new DocumentApplicationProperty() { Name = "key2", Value = "Delete" });

                Platform.ServerClient.Document document =
                    fileCabinet.EasyUploadDocument(new FileInfo[] { new FileInfo("") }, metaDocument);
            }
        }

        private static void CreateSectionApplicationProperties(Organization organization)
        {
            Console.WriteLine("CreateSectionApplicationProperties");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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



                    Platform.ServerClient.Section section = document.Sections.FirstOrDefault();
                    if (section == null)
                    {
                        Console.WriteLine("Section is null!");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                        documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                        documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key1", Value = "Test" });
                        documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key2", Value = "Delete" });

                        DocumentApplicationProperties resultDocumentApplicationProperties = section.PostToAppPropertiesRelationForDocumentApplicationProperties(
                            documentApplicationProperties);
                    }
                }
            }
        }

        private static void UpdateDocumentApplicationProperties(Organization organization)
        {
            Console.WriteLine("UpdateDocumentApplicationProperties");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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

                    DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                    documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                    documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key1", Value = "Test Update" });
                    documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key3", Value = "New Test" });

                    DocumentApplicationProperties resultDocumentApplicationProperties = document.PostToAppPropertiesRelationForDocumentApplicationProperties(documentApplicationProperties);
                }
            }
        }

        private static void UpdateSectionApplicationProperties(Organization organization)
        {
            Console.WriteLine("UpdateSectionApplicationProperties");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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



                    Platform.ServerClient.Section section = document.Sections.FirstOrDefault();
                    if (section == null)
                    {
                        Console.WriteLine("Section is null!");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                        documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                        documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key1", Value = "Test Update" });
                        documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key3", Value = "New Test" });

                        DocumentApplicationProperties resultDocumentApplicationProperties = section.PostToAppPropertiesRelationForDocumentApplicationProperties(
                            documentApplicationProperties);
                    }
                }
            }
        }

        private static void DeleteDocumentApplicationProperties(Organization organization)
        {
            Console.WriteLine("DeleteDocumentApplicationProperties");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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

                    DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                    documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                    documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key2", Value = null });

                    DocumentApplicationProperties resultDocumentApplicationProperties = document.PostToAppPropertiesRelationForDocumentApplicationProperties(documentApplicationProperties);
                }
            }
        }

        private static void DeleteSectionApplicationProperties(Organization organization)
        {
            Console.WriteLine("DeleteSectionApplicationProperties");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

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

                    Platform.ServerClient.Section section = document.Sections.FirstOrDefault();
                    if (section == null)
                    {
                        Console.WriteLine("Section is null!");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        DocumentApplicationProperties documentApplicationProperties = new DocumentApplicationProperties();
                        documentApplicationProperties.DocumentApplicationProperty = new List<DocumentApplicationProperty>();
                        documentApplicationProperties.DocumentApplicationProperty.Add(new DocumentApplicationProperty() { Name = "key2", Value = null });

                        DocumentApplicationProperties resultDocumentApplicationProperties = section.PostToAppPropertiesRelationForDocumentApplicationProperties(
                            documentApplicationProperties);
                    }
                }
            }
        }
    }
}
