using System;
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
                Organization organization = authenticator.ServiceConnection.Organizations.FirstOrDefault();
                if (organization == null)
                {
                    Console.WriteLine("No organization found");
                }
                else
                {
                    ListAll(organization);
                    ListByID(organization, "00000000-0000-0000-0000-000000000000");
                    ListByName(organization, "Invoices");
                }
            }
            Console.Read();
        }

        static void ListAll(Organization organization)
        {
            Console.WriteLine("ListAll FileCabinets");
            Console.WriteLine($"Organization {organization.Name} - FileCabinets");

            organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.ForEach(fc => Console.WriteLine($"ID {fc.Id} - Name {fc.Name}"));
        }

        static void ListByID(Organization organization, string fileCabinetId)
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

        static void ListByName(Organization organization, string fileCabinetName)
        {
            Console.WriteLine($"List FileCabinet by fileCabinetName {fileCabinetName}");

            Platform.ServerClient.FileCabinet fileCabinet = organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.FirstOrDefault(fc => fc.Name == fileCabinetName);

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
    }
}
