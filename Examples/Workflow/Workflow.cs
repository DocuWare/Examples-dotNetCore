using System;
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
                Organization organization = authenticator.ServiceConnection.Organizations.FirstOrDefault();
                if (organization == null)
                {
                    Console.WriteLine("No organization found");
                }
                else
                {
                    ListAllWorkflows(organization);
                    ListAllTasksByWorkflowId( organization, "00000000-0000-0000-0000-000000000000");
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
    }
}
