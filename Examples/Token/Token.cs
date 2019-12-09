using DocuWare.Platform.ServerClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class Token
    {
        private static string serverName = @"";
        private static string serverAddress = @"http://" + serverName + @"/DocuWare/Platform/";
        private static string userName = "";
        private static string userPassword = "";
        static void Main(string[] args)
        {
            Console.WriteLine("Token examples!");

            using (Helpers.Authenticator authenticator = new Helpers.Authenticator(serverAddress, userName, userPassword))
            {
                Organization organization = authenticator.ServiceConnection.Organizations.FirstOrDefault()?.GetOrganizationFromSelfRelation();
                if (organization == null)
                {
                    Console.WriteLine("No organization found");
                }
                else
                {
                    string token = CreateSingleToken(organization);

                    LoginWithTokenAndListAllFileCabinets(token);
                }
            }
            Console.Read();
        }

        private static string CreateSingleToken(Organization organization)
        {
            Console.WriteLine("CreateSingleToken");
            //Create single token
            string token = organization.PostToLoginTokenRelationForString(new TokenDescription()
            {
                Lifetime = TimeSpan.FromMinutes(1).ToString(),
                Usage = TokenUsage.Single,
                TargetProducts = new List<DWProductTypes>() { DWProductTypes.PlatformService }
            });

            Console.WriteLine($"Token= {token}");

            return token;
        }

        private static void LoginWithTokenAndListAllFileCabinets(string token)
        {
            Console.WriteLine("LoginWithTokenAndListAllFileCabinets");

            ServiceConnection serviceConnection = ServiceConnection.Create(new Uri(serverAddress), token);
            Organization organization = serviceConnection.Organizations.FirstOrDefault();

            Console.WriteLine($"Organization {organization.Name} - FileCabinets");
            organization.GetFileCabinetsFromFilecabinetsRelation().FileCabinet.ForEach(fc => Console.WriteLine($"ID= {fc.Id} Name= {fc.Name}"));

            serviceConnection.Disconnect();
        }
    }
}
