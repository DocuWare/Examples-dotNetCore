using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DocuWare.Platform.ServerClient;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class FileCabinet
    {
        static void Main(string[] args)
        {
            Console.WriteLine("FileCabinet Examples!");

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
                    ListAll(organization);
                    ListAllDocumentTrays(organization);
                    ListAllFileCabinets(organization);
                    ListDefaultDocumentTray(organization);
                    ListByID(organization, "00000000-0000-0000-0000-000000000000");
                    ListByName(organization, "Invoices");
                    UploadDocument(organization);
                    UploadDocumentWithTableField(organization);
                }
            }
            Console.Read();
        }

        private static void ListAll(Organization organization)
        {
            Console.WriteLine("ListAll FileCabinets and Document Trays");
            Console.WriteLine($"Organization {organization.Name} - FileCabinets / Document Trays");

            //List all FileCabinets
            organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .ForEach(fc => Console.WriteLine($"ID {fc.Id} - Name {fc.Name}"));
        }

        private static void ListAllDocumentTrays(Organization organization)
        {
            Console.WriteLine("ListAllDocumentTrays");
            Console.WriteLine($"Organization {organization.Name} - Document Trays");

            //List all FileCabinets with IsBasket flag true
            organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .Where(fc => fc.IsBasket)
                .ToList()
                .ForEach(fc => Console.WriteLine($"ID {fc.Id} - Name {fc.Name}"));
        }

        private static void ListAllFileCabinets(Organization organization)
        {
            Console.WriteLine("ListAllFileCabinets");
            Console.WriteLine($"Organization {organization.Name} - File Cabinets");

            //List all FileCabinets with IsBasket flag false
            organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .Where(fc => !fc.IsBasket)
                .ToList()
                .ForEach(fc => Console.WriteLine($"ID {fc.Id} - Name {fc.Name}"));
        }

        private static void ListDefaultDocumentTray(Organization organization)
        {
            Console.WriteLine("ListDefaultDocumentTray");

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.IsBasket && fc.Default);

            if (fileCabinet == null)
            {
                Console.WriteLine("Document Tray is null!");
            }
            else
            {
                Console.WriteLine($"Default FileCabinet ID: {fileCabinet.Id} - Name: {fileCabinet.Name}");
            }
        }

        private static void ListByID(Organization organization, string fileCabinetId)
        {
            Console.WriteLine($"List FileCabinet by fileCabinetId {fileCabinetId}");

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Console.WriteLine($"Organization {organization.Name} - FileCabinets");
                Console.WriteLine($"ID {fileCabinet.Id} - Name {fileCabinet.Name}");
            }
        }

        private static void ListByName(Organization organization, string fileCabinetName)
        {
            Console.WriteLine($"List FileCabinet by fileCabinetName {fileCabinetName}");

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Name == fileCabinetName);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Console.WriteLine($"Organization {organization.Name} - FileCabinets");
                Console.WriteLine($"ID {fileCabinet.Id} - Name {fileCabinet.Name}");
            }
        }

        private static void UploadDocument(Organization organization)
        {
            Console.WriteLine("UploadDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string fileInfoPath = @"C:\Temp\TestChange.json";

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Document metaDocument = new Document()
                {
                    Fields = new List<DocumentIndexField>()
                    {
                        //Create index value => field name, value
                        DocumentIndexField.Create("NAME", "TestUploadChange")
                    }
                };

                fileCabinet.EasyUploadDocument(new FileInfo[] {new FileInfo(fileInfoPath),},
                    metaDocument);
            }
        }

        private static void UploadDocumentWithTableField(Organization organization)
        {
            Console.WriteLine("UploadDocumentWithTableField");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            string fileInfoPath = @"C:\Temp\cat.jpg";

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                fileCabinet = fileCabinet.GetFileCabinetFromSelfRelation();
                List<FileCabinetField> fields = fileCabinet.Fields;

                foreach (var tableField in fields.Where(f => f.DWFieldType == DWFieldType.Table))
                {
                    List<FileCabinetFieldBase> tableFieldColumns = tableField.TableFieldColumns;

                    foreach (var tableFieldColumn in tableFieldColumns)
                    {
                        var Label = tableFieldColumn.DisplayName;
                        var DBFieldName = tableFieldColumn.DBFieldName;
                    }
                }

                DocumentIndexField tableDocumentIndexField = new DocumentIndexField()
                {
                    FieldName = "ANCESTRY",
                    ItemElementName = ItemChoiceType.Table,
                    Item = new DocumentIndexFieldTable()
                    {
                        Row = new List<DocumentIndexFieldTableRow>
                        {
                            new DocumentIndexFieldTableRow()
                            {
                                ColumnValue = new List<DocumentIndexField>()
                                {
                                    DocumentIndexField.Create("FIELD_NAME", "Mupil"),
                                    DocumentIndexField.Create("ANCES_RELATION", "Father"),
                                    DocumentIndexField.CreateDate("ANCES_BIRTHDAY", DateTime.Now.AddYears(-7)),
                                    DocumentIndexField.Create("ANCES_WEIGHT", 5)
                                }
                            },
                            new DocumentIndexFieldTableRow()
                            {
                                ColumnValue = new List<DocumentIndexField>()
                                {
                                    DocumentIndexField.Create("FIELD_NAME", "Fala"),
                                    DocumentIndexField.Create("ANCES_RELATION", "Mother"),
                                    DocumentIndexField.CreateDate("ANCES_BIRTHDAY", DateTime.Now.AddYears(-6)),
                                    DocumentIndexField.Create("ANCES_WEIGHT", 4.3m)
                                }
                            }
                        }
                    }
                };

                Document metaDocument = new Document()
                {
                    Fields = new List<DocumentIndexField>()
                    {
                        //Create index value => field name, value
                        DocumentIndexField.Create("NAME", "TestUpload with table field"),
                        tableDocumentIndexField
                    }
                };

                fileCabinet.EasyUploadDocument(new FileInfo[] { new FileInfo(fileInfoPath), },
                    metaDocument);
            }
        }

        
    }
}
