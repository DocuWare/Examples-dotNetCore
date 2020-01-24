using System;
using System.Collections.Generic;
using System.Linq;
using DocuWare.Platform.ServerClient;
using DocuWare.Services.Http.Client;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class Workflow
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Workflow Examples!");

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
                    ListAllWorkflows(organization);
                    ListAllTasksByWorkflowId(organization, "00000000-0000-0000-0000-000000000000");
                    ConfirmWorkflowTaskWithPrefilledValue(organization);
                    ConfirmWorkflowTaskValue(organization);
                }
            }
            Console.Read();
        }

        private static void ListAllWorkflows(Organization organization)
        {
            Console.WriteLine("ListAllWorkflows");
            Console.WriteLine("Workflows");
            organization.GetWorkflowsFromWorkflowsRelation().Workflow.ForEach(wf => Console.WriteLine($"Id: {wf.Id} - Name: {wf.Name}"));
        }

        private static void ListAllTasksByWorkflowId(Organization organization, string workflowId)
        {
            Console.WriteLine("ListAllTasksByWorkflowId");
            
            Platform.ServerClient.Workflow workflow = organization.GetWorkflowsFromWorkflowsRelation().Workflow.FirstOrDefault(wf =>
                wf.Id == workflowId)?.GetWorkflowFromSelfRelation();

            if (workflow == null)
            {
                Console.WriteLine("Workflow is null!");
            }
            else
            {
                Console.WriteLine("Tasks");
                workflow.GetWorkflowTasksFromTasksRelation().Task
                    .ForEach(t => Console.WriteLine($"Id: {t.Id} - description: {t.ActivityDescription}"));
            }
        }

        private static void ConfirmWorkflowTaskWithPrefilledValue(Organization organization)
        {
            Console.WriteLine("ConfirmWorkflowTasks");

            string workflowId = "00000000-0000-0000-0000-000000000000";

            Platform.ServerClient.Workflow workflow = organization.GetWorkflowsFromWorkflowsRelation().Workflow.FirstOrDefault(wf =>
                wf.Id == workflowId)?.GetWorkflowFromSelfRelation();

            if (workflow == null)
            {
                Console.WriteLine("Workflow is null!");
            }
            else
            {
                WorkflowTask task = workflow.GetWorkflowTasksFromTasksRelation().Task.FirstOrDefault();

                if (task == null)
                {
                    Console.WriteLine("Task is null!");
                }
                else
                {
                    task = task.TaskOperations.BaseTaskOperations.GetWorkflowTaskFromSelfRelation();

                    Decision firstDecision = task.Decisions.FirstOrDefault();

                    if (firstDecision == null)
                    {
                        Console.WriteLine("FirstDecision is null!");
                    }
                    else
                    {
                        //it returns all fields and prefills in the decision
                        Decision fullLoadedDecision = firstDecision.DecisionOperations.BaseDecisionOperations.GetDecisionFromSelfRelation();
                        var confirmedData = new ConfirmedData()
                        {
                            ConfirmedFields = new List<ConfirmedField>()
                        };
                        foreach (var field in fullLoadedDecision.TaskFormField.Where(f => f.Item.FormFieldType != FormTypeEnum.Description && f.Item.FormFieldType != FormTypeEnum.Link))
                        {
                            confirmedData.ConfirmedFields.Add(new ConfirmedField()
                            {
                                Id = field.Item.Id,
                                Value = GetPrefilledFieldValue(field)
                            });
                        }
                        string returnValue = fullLoadedDecision.DecisionOperations.ExtendedDecisionOperations.PostToConfirmRelationForString(confirmedData);
                    }
                }
            }
        }

        private static WFFormFieldValue GetPrefilledFieldValue(TaskFormField field)
        {
            switch (field.Item)
            {
                case KeywordsField keywordsField: return keywordsField.PrefillValue;
                case TaskNumberField taskNumberField: return taskNumberField.PrefillValue;
                case RoleField roleField: return roleField.PrefillValue;
                case SubstitutionRuleField substitutionRuleField: return substitutionRuleField.PrefillValue;
                case TaskDateTimeField taskDateTimeField: return taskDateTimeField.PrefillValue;
                case TaskTextField taskTextField: return taskTextField.PrefillValue;
                case UserField userField: return userField.PrefillValue;
                default: return null;

            }
        }

        private static void ConfirmWorkflowTaskValue(Organization organization)
        {
            Console.WriteLine("ConfirmWorkflowTasks");

            string workflowId = "00000000-0000-0000-0000-000000000000";

            Platform.ServerClient.Workflow workflow = organization.GetWorkflowsFromWorkflowsRelation().Workflow.FirstOrDefault(wf =>
                wf.Id == workflowId)?.GetWorkflowFromSelfRelation();

            if (workflow == null)
            {
                Console.WriteLine("Workflow is null!");
            }
            else
            {
                WorkflowTask task = workflow.GetWorkflowTasksFromTasksRelation().Task.FirstOrDefault();

                if (task == null)
                {
                    Console.WriteLine("Task is null!");
                }
                else
                {
                    task = task.TaskOperations.BaseTaskOperations.GetWorkflowTaskFromSelfRelation();

                    Decision firstDecision = task.Decisions.FirstOrDefault();

                    if (firstDecision == null)
                    {
                        Console.WriteLine("FirstDecision is null!");
                    }
                    else
                    {
                        //it returns all fields and prefills in the decision
                        Decision fullLoadedDecision = firstDecision.DecisionOperations.BaseDecisionOperations.GetDecisionFromSelfRelation();
                        var confirmedData = new ConfirmedData()
                        {
                            ConfirmedFields = new List<ConfirmedField>()
                        };
                        foreach (var field in fullLoadedDecision.TaskFormField.Where(f => f.Item.FormFieldType != FormTypeEnum.Description && f.Item.FormFieldType != FormTypeEnum.Link))
                        {
                            confirmedData.ConfirmedFields.Add(new ConfirmedField()
                            {
                                Id = field.Item.Id,
                                Value = GetExampleFieldValue(field)
                            });
                        }
                        string returnValue = fullLoadedDecision.DecisionOperations.ExtendedDecisionOperations.PostToConfirmRelationForString(confirmedData);
                    }
                }
            }
        }

        private static WFFormFieldValue GetExampleFieldValue(TaskFormField field)
        {
            switch (field.Item)
            {
                case KeywordsField keywordsField:
                    return new WFFormFieldValue() {Item = new DocumentIndexFieldKeywords() { Keyword = new List<string>() {"keyword 1"} },
                        ItemElementName = TranslateFromFormFieldType(keywordsField.FormFieldType)};
                case TaskNumberField taskNumberField:
                    return new WFFormFieldValue() {Item = 1m,
                        ItemElementName = TranslateFromFormFieldType(taskNumberField.FormFieldType)};
                case RoleField roleField:
                    return new WFFormFieldValue() {Item = roleField.IsMultiselect ? (object)new DocumentIndexFieldKeywords() { Keyword = new List<string>() { "role1", "role2" } } : "role",
                        ItemElementName = TranslateFromFormFieldType(roleField.FormFieldType)};
                case SubstitutionRuleField substitutionRuleField:
                    return new WFFormFieldValue() {Item = substitutionRuleField.IsMultiselect ? (object)new DocumentIndexFieldKeywords() { Keyword = new List<string>() { "substitutionRule1", "substitutionRule2" } } : "substitutionRule",
                        ItemElementName = TranslateFromFormFieldType(substitutionRuleField.FormFieldType)};
                case TaskDateTimeField taskDateTimeField:
                    return new WFFormFieldValue() {Item = new DateTime(),
                    ItemElementName = TranslateFromFormFieldType(taskDateTimeField.FormFieldType)};
                case TaskTextField taskTextField:
                    return new WFFormFieldValue() {Item = "text field",
                        ItemElementName = TranslateFromFormFieldType(taskTextField.FormFieldType)};
                case UserField userField:
                    return new WFFormFieldValue() {Item = userField.IsMultiselect ? (object) new DocumentIndexFieldKeywords(){Keyword = new List<string>() { "user1", "user2"} } : "user",
                        ItemElementName = TranslateFromFormFieldType(userField.FormFieldType)};
                default: return null;

            }
        }

        private static ItemChoiceType TranslateFromFormFieldType(FormTypeEnum formType)
        {
            switch (formType)
            {
                case FormTypeEnum.Date:
                    return ItemChoiceType.Date;
                case FormTypeEnum.DateTime:
                    return ItemChoiceType.DateTime;
                case FormTypeEnum.Decimal:
                    return ItemChoiceType.Decimal;
                case FormTypeEnum.Description:
                    return ItemChoiceType.Memo;
                case FormTypeEnum.Password:
                    return ItemChoiceType.String;
                case FormTypeEnum.Table:
                    return ItemChoiceType.Table;
                case FormTypeEnum.Text:
                    return ItemChoiceType.String;
                case FormTypeEnum.SubstitutionRule:
                case FormTypeEnum.Role:
                case FormTypeEnum.User:
                case FormTypeEnum.Keyword:
                    return ItemChoiceType.Keywords;
            }
            return ItemChoiceType.String;
        }

    }
}
