using DocuWare.Platform.ServerClient;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DocuWare.SDK.Samples.dotNetCore.Examples
{
    class Users
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Users Examples!");

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
                    CreateUser(organization);
                    AddRoleToUser(organization);
                    RemoveRoleFromUser(organization);
                    AddGroupToUser(organization);
                    RemoveGroupFromUser(organization);
                }
            }

            Console.Read();
        }

        private static void CreateUser(Organization organization)
        {
            Console.WriteLine("CreateUser");

            string userName = "tester";
            string email = "test@test.com";
            string password = "test#2020";

            NewUser newUser = new NewUser()
            {
                DbName = userName,
                Name = userName,
                Email = email,
                NetworkId = "",
                Password = password
            };

            User user = organization.PostToUserInfoRelationForUser(newUser);
        }

        private static void AddRoleToUser(Organization organization)
        {
            Console.WriteLine("AddRoleToUser");

            string userName = "tester";
            string roleId = "00000000-0000-0000-0000-000000000000";

            User user = organization.GetUsersFromUsersRelation().User.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                Console.WriteLine("User is null!");
            }
            else
            {
                user.PutToRolesRelationForString(new AssignmentOperation()
                {
                    OperationType = AssignmentOperationType.Add,
                    Ids = new List<string>()
                    {
                        roleId
                    }
                });
            }
        }

        private static void RemoveRoleFromUser(Organization organization)
        {
            Console.WriteLine("RemoveRoleFromUser");

            string userName = "tester";
            string roleId = "00000000-0000-0000-0000-000000000000";

            User user = organization.GetUsersFromUsersRelation().User.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                Console.WriteLine("User is null!");
            }
            else
            {
                user.PutToRolesRelationForString(new AssignmentOperation()
                {
                    OperationType = AssignmentOperationType.Remove,
                    Ids = new List<string>()
                    {
                        roleId
                    }
                });
            }
        }

        private static void AddGroupToUser(Organization organization)
        {
            Console.WriteLine("AddGroupToUser");

            string userName = "tester";
            string groupId = "00000000-0000-0000-0000-000000000000";

            User user = organization.GetUsersFromUsersRelation().User.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                Console.WriteLine("User is null!");
            }
            else
            {
                user.PutToGroupsRelationForString(new AssignmentOperation()
                {
                    OperationType = AssignmentOperationType.Add,
                    Ids = new List<string>()
                    {
                        groupId
                    }
                });
            }
        }

        private static void RemoveGroupFromUser(Organization organization)
        {
            Console.WriteLine("RemoveGroupFromUser");

            string userName = "tester";
            string groupId = "00000000-0000-0000-0000-000000000000";

            User user = organization.GetUsersFromUsersRelation().User.FirstOrDefault(u => u.Name == userName);

            if (user == null)
            {
                Console.WriteLine("User is null!");
            }
            else
            {
                user.PutToGroupsRelationForString(new AssignmentOperation()
                {
                    OperationType = AssignmentOperationType.Remove,
                    Ids = new List<string>()
                    {
                        groupId
                    }
                });
            }
        }
    }
}
