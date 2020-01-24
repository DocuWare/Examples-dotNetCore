using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using DocuWare.Platform.ServerClient;
using DocuWare.Services.Http;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    partial class Document
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Document Examples!");

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
                    ListAllDocuments(organization);
                    MergeDocuments(organization);
                    DivideDocuments(organization);
                    UpdateIndexFields(organization);
                    UpdateAllIndexFields(organization);
                    UpdateIndexFieldsWithTableField(organization);
                    Download(organization);
                    DownloadSection(organization);
                    UploadSection(organization);
                    EditSection(organization);
                    await CheckOutCheckIn(authenticator.ServiceConnection);
                    TransferFromDocumentTrayToFileCabinet(organization);
                    TransferFromFileCabinetToFileCabinetWithFields(organization);
                    await LockDocument(organization);
                    //Import and export dwx archive
                    ExportArchivedDocument(organization);
                    ExportArchivedDocuments(organization);
                    ImportArchivedDocument(organization);
                    //ApplicationProperties
                    CreateDocumentApplicationProperties(organization);
                    CreateDocumentApplicationPropertiesWithNewDocument(organization);
                    CreateSectionApplicationProperties(organization);
                    UpdateDocumentApplicationProperties(organization);
                    UpdateSectionApplicationProperties(organization);
                    DeleteDocumentApplicationProperties(organization);
                    DeleteSectionApplicationProperties(organization);
                    //Stamps and annotations
                    SetStampOnPageWithSpecificPosition(organization);
                    SetStampOnPageWithBestPosition(organization);
                    SetStampOnDocumentWithSpecificPosition(organization);
                    SetAnnotationOnPage(organization);
                    SetAnnotationOnDocument(organization);
                    //Replace
                    ReplaceAllSectionsInDocument(organization);
                    ReplaceSpecificSectionInDocument(organization);
                }
            }
            Console.Read();
        }

        private static void ListAllDocuments(Organization organization)
        {
            Console.WriteLine("ListAllDocuments");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Console.WriteLine("Documents");
                //Be aware: Only first X documents are returned per default.
                fileCabinet.GetDocumentsQueryResultFromDocumentsRelation().Items.ForEach(d => Console.WriteLine($"ID: {d.Id}"));
            }
        }

        private static void MergeDocuments(Organization organization)
        {
            Console.WriteLine("MergeDocuments");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            List<int> documentIds = new List<int>() { 1, 2 };

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

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
                    ContentDivideOperationInfo contentDivideOperationInfo = new ContentDivideOperationInfo()
                    {
                        Operation = ContentDivideOperation.Unstaple,
                        Force = true
                    };

                    DocumentsQueryResult dividedDocuments = document.PutToContentDivideOperationRelationForDocumentsQueryResult(contentDivideOperationInfo);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="organization"></param>
        private static void UpdateIndexFields(Organization organization)
        {
            Console.WriteLine("UpdateIndexFields");

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
                    DocumentIndexFields documentIndexFields = new DocumentIndexFields()
                    {
                        Field = new List<DocumentIndexField>()
                        {
                            DocumentIndexField.Create("NAME", "TestChange"),
                            DocumentIndexField.CreateDate("DATE_OF_BIRTH", DateTime.Now)
                        }
                    };
      
                    DocumentIndexFields result =
                        document.PutToFieldsRelationForDocumentIndexFields(documentIndexFields);

                    Console.WriteLine($"Results");
                    result.Field.ForEach(f => Console.WriteLine($"FiledName: {f.FieldName} - Item: {f.Item}"));
                }
            }
        }

        private static void UpdateIndexFieldsWithTableField(Organization organization)
        {
            Console.WriteLine("UpdateIndexFieldsWithTableField");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

            string tableFieldName = "ANCESTRY";
            string tableFieldSearchColumnName = "FIELD_NAME";
            string tableFieldSearchColumnValue = "";
            string tableFieldChangeColumnName = "ANCES_WEIGHT";
            decimal tableFieldChangeColumnValue = 4.5m;

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

                    DocumentIndexField tableDocumentIndexField =
                        document.Fields.FirstOrDefault(f =>
                            f.FieldName == tableFieldName && f.ItemElementName == ItemChoiceType.Table);

                    if (tableDocumentIndexField == null)
                    {
                        Console.WriteLine("TableDocumentIndexField is null!");
                    }
                    else
                    {
                        DocumentIndexFieldTable existingDocumentIndexFieldTable = tableDocumentIndexField.Item as DocumentIndexFieldTable;

                        if (existingDocumentIndexFieldTable == null)
                        {
                            Console.WriteLine("ExistingDocumentIndexFieldTable is null!");
                        }
                        else if (existingDocumentIndexFieldTable.Row.Count < 1)
                        {
                            Console.WriteLine("ExistingDocumentIndexFieldTable Row count is 0.");
                        }
                        else
                        {
                            DocumentIndexFieldTableRow documentIndexFieldTableRow =
                                existingDocumentIndexFieldTable.Row.FirstOrDefault(r =>
                                    r.ColumnValue.Exists(c => c.FieldName == tableFieldSearchColumnName && (string)c.Item == tableFieldSearchColumnValue));

                            DocumentIndexField columnDocumentIndexField =
                                documentIndexFieldTableRow?.ColumnValue.FirstOrDefault(c => c.FieldName == tableFieldChangeColumnName);

                            if (columnDocumentIndexField == null)
                            {
                                Console.WriteLine("ColumnDocumentIndexField is null!");
                            }
                            else
                            {
                                columnDocumentIndexField.Item = tableFieldChangeColumnValue;

                                DocumentIndexFields updatedTableIndexFields = new DocumentIndexFields()
                                {
                                    Field = new List<DocumentIndexField>()
                                    {
                                        tableDocumentIndexField
                                    }
                                };

                                DocumentIndexFields documentIndexField =
                                    document.PutToFieldsRelationForDocumentIndexFields(updatedTableIndexFields);
                            }
                        }
                    }

                }
            }
        }

        private static void UpdateAllIndexFields(Organization organization)
        {
            Console.WriteLine("UpdateIndexFields");

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
                    
                    DocumentIndexFields fields = document.GetDocumentIndexFieldsFromFieldsRelation();
                    fileCabinet = fileCabinet.GetFileCabinetFromSelfRelation();
                    List<FileCabinetField> fileCabinetFields = fileCabinet.Fields;
                    fields.Field.ForEach(f =>
                    {
                        // Set correct field type
                        f.ItemElementName = GetCorrectItemChoiceTypeFromFileCabinetFields(f, fileCabinetFields);
                        // Change Value
                        if (f.FieldName == "NAME")
                        {
                            f.Item = "Change all test";
                        }
                    });

                    DocumentIndexFields result =
                        document.PutToFieldsRelationForDocumentIndexFields(fields);

                    Console.WriteLine($"Results");
                    result.Field.ForEach(f => Console.WriteLine($"FiledName: {f.FieldName} - Item: {f.Item}"));
                }
            }
        }
        
        private static ItemChoiceType GetCorrectItemChoiceTypeFromFileCabinetFields(DocumentIndexField field, List<FileCabinetField> fileCabinetFields)
        {
            FileCabinetField fileCabinetField = fileCabinetFields.FirstOrDefault(f => f.DBFieldName == field.FieldName);

            //Do nothing in case of field is not a null value
            if (field.IsNull == false)
            {
                return field.ItemElementName;
            }

            // Change nothing by default because of fields who aren't in the FileCabinetFields
            switch (fileCabinetField?.DWFieldType)
            {
                case DWFieldType.Date:
                    return ItemChoiceType.Date;
                case DWFieldType.DateTime:
                    return ItemChoiceType.DateTime;
                case DWFieldType.Decimal:
                    return ItemChoiceType.Decimal;
                case DWFieldType.Keyword:
                    return ItemChoiceType.Keywords;
                case DWFieldType.Memo:
                    return ItemChoiceType.Memo;
                case DWFieldType.Numeric:
                    return ItemChoiceType.Int;
                case DWFieldType.Table:
                    return ItemChoiceType.Table;
                case DWFieldType.Text:
                    return ItemChoiceType.String;
                default:
                    return field.ItemElementName;
            }
        }

        private static void Download(Organization organization)
        {
            Console.WriteLine("Download");

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

                    DeserializedHttpResponse<Stream> deserializedHttpResponse = document.PostToFileDownloadRelationForStreamAsync(new FileDownload()
                    {
                        TargetFileType = FileDownloadType.Auto // FileDownloadType.PDF / FileDownloadType.ZIP
                    }).Result;

                    HttpContentHeaders httpContentHeaders = deserializedHttpResponse.ContentHeaders;

                    string ContentType = httpContentHeaders.ContentType.MediaType;
                    string FileName = deserializedHttpResponse.GetFileName();
                    long? ContentLength = httpContentHeaders.ContentLength;
                    Stream stream = deserializedHttpResponse.Content;

                    using (FileStream fileStream = File.Create(Path.Combine(@"C:\Temp\", FileName)))
                    {
                        if (stream.CanSeek)
                        {
                            stream.Seek(0, SeekOrigin.Begin);
                        }
                        stream.CopyTo(fileStream);
                    }
                }
            }
        }

        private static void DownloadSection(Organization organization)
        {
            Console.WriteLine("DownloadSection");

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

                    if (document.Sections.Count < 1)
                    {
                        Console.WriteLine("Document has not enough sections!");
                    }
                    else
                    {
                        Section section = document.Sections[1];

                        section = section.GetSectionFromSelfRelation();

                        DeserializedHttpResponse<Stream> deserializedHttpResponse = section.PostToFileDownloadRelationForStreamAsync(new FileDownload()
                        {
                            TargetFileType = FileDownloadType.Auto
                        }).Result;

                        HttpContentHeaders httpContentHeaders = deserializedHttpResponse.ContentHeaders;

                        string ContentType = httpContentHeaders.ContentType.MediaType;
                        string FileName = deserializedHttpResponse.GetFileName();
                        long? ContentLength = httpContentHeaders.ContentLength;
                        Stream stream = deserializedHttpResponse.Content;

                        using (FileStream fileStream = File.Create(Path.Combine(@"C:\Temp\", FileName)))
                        {
                            if (stream.CanSeek)
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            stream.CopyTo(fileStream);
                        } 
                    }
                }
            }
        }

        private static void UploadSection(Organization organization)
        {
            Console.WriteLine("UploadSection");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string fileInfoPath = @"C:\Temp\TestChange.json";

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
                    document.EasyUploadFile(new FileInfo(fileInfoPath));
                }
            }
        }

        private static void EditSection(Organization organization)
        {
            Console.WriteLine("UpdateIndexFields");

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

                    var section = document.Sections.FirstOrDefault();

                    if (section == null)
                    {
                        Console.WriteLine("Section is null!");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        DeserializedHttpResponse<Stream> deserializedHttpResponse = section.PostToFileDownloadRelationForStreamAsync(new FileDownload()
                        {
                            TargetFileType = FileDownloadType.Auto // FileDownloadType.PDF / FileDownloadType.ZIP
                        }).Result;

                        HttpContentHeaders httpContentHeaders = deserializedHttpResponse.ContentHeaders;

                        string ContentType = httpContentHeaders.ContentType.MediaType;
                        string DirectoryPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        string FileName = Path.Combine(DirectoryPath, deserializedHttpResponse.GetFileName());
                        long? ContentLength = httpContentHeaders.ContentLength;
                        Stream stream = deserializedHttpResponse.Content;

                        Directory.CreateDirectory(DirectoryPath);

                        using (FileStream fileStream = File.Create(FileName))
                        {
                            if (stream.CanSeek)
                            {
                                stream.Seek(0, SeekOrigin.Begin);
                            }
                            stream.CopyTo(fileStream);
                        }

                        //edit your file here
                         
                        section.EasyReplaceFile(new FileInfo(FileName));

                        Directory.Delete(DirectoryPath, true);
                    }
                }
            }
        }

        private static async Task CheckOutCheckIn(ServiceConnection serviceConnection)
        {
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;

            FileInfo fileInfo = null;

            //Check out
            using (var result = await serviceConnection.EasyCheckOutToFileSystemAsync(fileCabinetId, documentId)
                .ConfigureAwait(false))
            {
                var tempPath = Path.Combine(Path.GetTempPath(), result.EncodedFileName);

                using (var file = System.IO.File.Create(tempPath))
                using (var stream = result.Response.Content)
                    await stream.CopyToAsync(file).ConfigureAwait(false);

                fileInfo = new FileInfo(tempPath);
            }

            //Do your stuff you want to do with the file
            await Task.Delay(2000).ConfigureAwait(false);

            //Check in
            CheckInActionParameters checkInActionParameters = new CheckInActionParameters()
            {
                Comments = "comments...",
                DocumentVersion = new DocumentVersion()
                {
                    Major = 1,
                    Minor = 2

                }
            };
            DeserializedHttpResponse<Platform.ServerClient.Document> document = await serviceConnection.EasyCheckInFromFileSystemAsync(fileInfo, checkInActionParameters).ConfigureAwait(false);
        }

        private static void TransferFromDocumentTrayToFileCabinet(Organization organization)
        {
            Console.WriteLine("TransferFromDocumentTrayToFileCabinet");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            List<int> documentIds = new List<int>()
            {
                8
            };
            string webBasketId = "b_00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                FileCabinetTransferInfo fileCabinetTransferInfo = new FileCabinetTransferInfo()
                {
                    KeepSource = true,
                    SourceDocId = documentIds,
                    SourceFileCabinetId = webBasketId
                };

                DocumentsQueryResult documentsQueryResult =
                    fileCabinet.PostToTransferRelationForDocumentsQueryResult(fileCabinetTransferInfo);
            }
        }

        private static void TransferFromFileCabinetToFileCabinet(Organization organization)
        {
            Console.WriteLine("TransferFromFileCabinetToFileCabinet");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            List<int> documentIds = new List<int>()
            {
                8
            };
            string sourceFileCabinetId = "00000000-0000-0000-0000-000000000000";

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                FileCabinetTransferInfo fileCabinetTransferInfo = new FileCabinetTransferInfo()
                {
                    KeepSource = true,
                    SourceDocId = documentIds,
                    SourceFileCabinetId = sourceFileCabinetId
                };

                DocumentsQueryResult documentsQueryResult =
                    fileCabinet.PostToTransferRelationForDocumentsQueryResult(fileCabinetTransferInfo);
            }
        }

        private static void TransferFromFileCabinetToFileCabinetWithFields(Organization organization)
        {
            Console.WriteLine("TransferFromFileCabinetToFileCabinetWithFields");
            string destinationFileCabinetId = "00000000-0000-0000-0000-000000000000";
            string sourceFileCabinetId = "00000000-0000-0000-0000-000000000000";
            int sourceDocId = 6;

            FileCabinet destinationFileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == destinationFileCabinetId);

            if (destinationFileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document sourceDocument = new Platform.ServerClient.Document
                {
                    Id = sourceDocId,
                    Fields = new List<DocumentIndexField>()
                    {
                        DocumentIndexField.Create("NAME", "TestName"),
                        DocumentIndexField.Create("COLOR", "White/Red")
                    }
                };

                DocumentsTransferInfo documentsTransferInfo = new DocumentsTransferInfo()
                {
                    Documents = new List<Platform.ServerClient.Document>()
                    {
                        sourceDocument
                    },
                    KeepSource = true,
                    SourceFileCabinetId = sourceFileCabinetId
                };

                DocumentsQueryResult documentsQueryResult =
                    destinationFileCabinet.PostToTransferRelationForDocumentsQueryResult(documentsTransferInfo);
            }
        }

        private static async Task LockDocument(Organization organization)
        {
            string clientIdentifier = "SampleDocumentLockApplication";

            //DocumentLock
            Console.WriteLine("LockDocument");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string fileInfoPath = @"C:\Temp\TestChange.json";

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

                    DocumentLock documentLock = await document.LockAsync((exception) =>
                    {
                        Console.WriteLine(exception.Message);
                    }, clientIdentifier, 60);

                    using (documentLock)
                    {
                        await document.EasyUploadFileAsync(new FileInfo(fileInfoPath));
                    }
                }
            }
        }
    }
}
