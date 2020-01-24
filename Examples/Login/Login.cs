using System;
using System.Linq;
using DocuWare.Platform.ServerClient;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class Login
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Login examples!");

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
                    Console.WriteLine($"Organization {organization.Name} found");
                }
            }

            Console.Read();
        }
    }
}
