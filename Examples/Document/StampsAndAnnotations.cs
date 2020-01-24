using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocuWare.Platform.ServerClient;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    partial class Document
    {
        private static void SetStampOnPageWithSpecificPosition(Organization organization)
        {
            Console.WriteLine("SetStampOnPageWithSpecificPosition");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string SectionId = "1-1";
            string stampId = "00000000-0000-0000-0000-000000000000";
            int layer = 1; //Layer can be 1 to 5
            double locationX = 100;
            double locationY = 100;
            string itemValue = "December";

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

                    var section = document.Sections.FirstOrDefault(s => s.Id == SectionId);

                    if (section == null)
                    {
                        Console.WriteLine("Section is null");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        var page = section.Pages.GetPagesFromNextBlockRelation().Page.FirstOrDefault();

                        if (page == null)
                        {
                            Console.WriteLine("Page is null!");
                        }
                        else
                        {
                            StampPlacement stampPlacement = new StampPlacement()
                            {
                                StampId = stampId,
                                Layer = layer,
                                Location = new DWPoint()
                                {
                                    X = locationX,
                                    Y = locationY 

                                },
                                Field = new List<FormFieldValue>()
                                {
                                    new FormFieldValue()
                                    {
                                        Name = "<#1>",
                                        TypedValue = new DocumentIndexFieldValue()
                                        {
                                            ItemElementName = ItemChoiceType.String,
                                            Item = itemValue
                                        }
                                    }
                                }
                            };

                            Annotation annotation = page.PostToStampRelationForAnnotation(stampPlacement);
                        }

                    }
                }
            }
        }

        private static void SetStampOnPageWithBestPosition(Organization organization)
        {
            Console.WriteLine("SetStampOnPageWithBestPosition");

            string queryDialogId = "00000000-0000-0000-0000-000000000000";
            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string SectionId = "1-1";
            string stampId = "00000000-0000-0000-0000-000000000000";
            int layer = 1; //Layer can be 1 to 5
            DWPoint bestPosition;
            string itemValue = "December";

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

                    Section section = document.Sections.FirstOrDefault(s => s.Id == SectionId);

                    if (section == null)
                    {
                        Console.WriteLine("Section is null");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        Page page = section.Pages.GetPagesFromNextBlockRelation().Page.FirstOrDefault();

                        if (page == null)
                        {
                            Console.WriteLine("Page is null!");
                        }
                        else
                        {
                            bestPosition = page.PostToStampBestPositionRelationForDWPoint(new StampFormFieldValues() { StampId = stampId });

                            if (bestPosition == null)
                            {
                                Console.WriteLine("BestPositon is null!");
                            }
                            else
                            {
                                StampPlacement stampPlacement = new StampPlacement()
                                {
                                    StampId = stampId,
                                    Layer = layer,
                                    Location = bestPosition,
                                    Field = new List<FormFieldValue>()
                                    {
                                        new FormFieldValue()
                                        {
                                            Name = "<#1>",
                                            TypedValue = new DocumentIndexFieldValue()
                                            {
                                                ItemElementName = ItemChoiceType.String,
                                                Item = itemValue
                                            }
                                        }
                                    }
                                };

                                Annotation annotation = page.PostToStampRelationForAnnotation(stampPlacement);
                            }


                        }

                    }
                }
            }
        }

        private static void SetStampOnDocumentWithSpecificPosition(Organization organization)
        {
            Console.WriteLine("SetStampOnDocumentWithSpecificPosition");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string stampId = "00000000-0000-0000-0000-000000000000";
            int layer = 2; //Layer can be 1 to 5
            double locationX = 100;
            double locationY = 100;
            string itemValue = "December";
            int pageNumber = 0;
            int sectionNumber = 0;

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation()
                    .Items
                    .FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    document = document.GetDocumentFromSelfRelation();


                    StampPlacement stampPlacement = new StampPlacement()
                    {
                        StampId = stampId,
                        Layer = layer,
                        Location = new DWPoint()
                        {
                            X = locationX,
                            Y = locationY 

                        },
                        Field = new List<FormFieldValue>()
                        {
                            new FormFieldValue()
                            {
                                Name = "<#1>",
                                TypedValue = new DocumentIndexFieldValue()
                                {
                                    ItemElementName = ItemChoiceType.String,
                                    Item = itemValue
                                }
                            }
                        }
                    };

                    DocumentAnnotationsPlacement documentAnnotationsPlacement = new DocumentAnnotationsPlacement()
                    {
                        Annotations = new List<SectionAnnotationsPlacement>()
                        {
                            new SectionAnnotationsPlacement()
                            {
                                PageNumber = pageNumber,
                                SectionNumber = sectionNumber,
                                AnnotationsPlacement = new AnnotationsPlacement()
                                {
                                    Items = new List<object>()
                                    {
                                        stampPlacement
                                    }
                                }
                            }
                        }
                    };

                    DocumentAnnotations documentAnnotations = document.PostToAnnotationRelationForDocumentAnnotations(documentAnnotationsPlacement);
                }
            }
        }

        private static void SetAnnotationOnPage(Organization organization)
        {
            Console.WriteLine("SetAnnotationOnPage");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 1;
            string SectionId = "1-1";
            int layer = 1; //Layer can be 1 to 5

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation()
                    .Items
                    .FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    document = document.GetDocumentFromSelfRelation();

                    var section = document.Sections.FirstOrDefault(s => s.Id == SectionId);

                    if (section == null)
                    {
                        Console.WriteLine("Section is null");
                    }
                    else
                    {
                        section = section.GetSectionFromSelfRelation();

                        var page = section.Pages.GetPagesFromNextBlockRelation().Page.FirstOrDefault();

                        if (page == null)
                        {
                            Console.WriteLine("Page is null!");
                        }
                        else
                        {
                            var annotations = new Annotation()
                            {
                                Layer = new List<Layer>()
                                {
                                    new Layer()
                                    {
                                        Id = layer,
                                        Items = new List<EntryBase>()
                                        {
                                            new TextEntry()
                                            {
                                                Location = new AnnotationRectangle()
                                                {
                                                    Left = 100,
                                                    Top = 100,
                                                    Width = 200,
                                                    Height = 200
                                                },
                                                Value = "Test text",
                                                Font = new Font()
                                                {
                                                    FontSize = 10 * 20,
                                                    FontName = "Arial"
                                                }
                                            },
                                            new RectEntry()
                                            {
                                                Location = new AnnotationRectangle()
                                                {
                                                    Left = 300,
                                                    Top = 300,
                                                    Width = 200,
                                                    Height = 200
                                                },
                                                Filled = true,
                                                Ellipse = true
                                            },
                                            new LineEntry()
                                            {
                                                From = new AnnotationPoint() { X = 500, Y = 500 },
                                                To = new AnnotationPoint() { X = 800, Y = 800 },
                                                Arrow = true,
                                            }
                                        }
                                    }
                                }
                            };

                            Annotation annotation = page.PostToAnnotationRelationForAnnotation(annotations);
                        }
                    }
                }
            }
        }

        private static void SetAnnotationOnDocument(Organization organization)
        {
            Console.WriteLine("SetAnnotationOnDocument");

            string fileCabinetId = "00000000-0000-0000-0000-000000000000";
            int documentId = 4;
            int pageNumber = 0;
            int sectionNumber = 0;
            int layer = 1; //Layer can be 1 to 5

            FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet
                .FirstOrDefault(fc => fc.Id == fileCabinetId);

            if (fileCabinet == null)
            {
                Console.WriteLine("FileCabinet is null!");
            }
            else
            {
                Platform.ServerClient.Document document = fileCabinet.GetDocumentsQueryResultFromDocumentsRelation()
                    .Items
                    .FirstOrDefault(d => d.Id == documentId);

                if (document == null)
                {
                    Console.WriteLine("Document is null!");
                }
                else
                {
                    document = document.GetDocumentFromSelfRelation();

                    Annotation annotation = new Annotation()
                    {
                        Layer = new List<Layer>()
                        {
                            new Layer()
                            {
                                Id = layer,
                                Items = new List<EntryBase>()
                                {
                                    new TextEntry()
                                    {
                                        Location = new AnnotationRectangle()
                                        {
                                            Left = 100,
                                            Top = 100,
                                            Width = 200,
                                            Height = 200
                                        },
                                        Value = "Test text",
                                        Font = new Font()
                                        {
                                            FontSize = 10 * 20,
                                            FontName = "Arial"
                                        }
                                    },
                                    new RectEntry()
                                    {
                                        Location = new AnnotationRectangle()
                                        {
                                            Left = 300,
                                            Top = 300,
                                            Width = 200,
                                            Height = 200
                                        },
                                        Filled = true,
                                        Ellipse = true
                                    },
                                    new LineEntry()
                                    {
                                        From = new AnnotationPoint() {X = 500, Y = 500},
                                        To = new AnnotationPoint() {X = 800, Y = 800},
                                        Arrow = true,
                                    }
                                }
                            }
                        }
                    };

                    DocumentAnnotationsPlacement documentAnnotationsPlacement = new DocumentAnnotationsPlacement()
                    {
                        Annotations = new List<SectionAnnotationsPlacement>()
                        {
                            new SectionAnnotationsPlacement()
                            {
                                PageNumber = pageNumber,
                                SectionNumber = sectionNumber,
                                AnnotationsPlacement = new AnnotationsPlacement()
                                {
                                    Items = new List<object>()
                                    {
                                       annotation
                                    }
                                }
                            }
                        }
                    };

                    DocumentAnnotations documentAnnotations = document.PostToAnnotationRelationForDocumentAnnotations(documentAnnotationsPlacement);
                }
            }
        }
    }
}
