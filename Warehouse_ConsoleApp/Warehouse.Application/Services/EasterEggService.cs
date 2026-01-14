using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Warehouse.Persistence.MsSql;

namespace Warehouse.Application
{
    public class User 
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class EasterEggService
    {
        string UserFilePath = "users.json";
        List<User> userDatabase = new List<User>();
        bool isLoggedIn = false;
        string loggedInUser = string.Empty;
        AllXmlDataExporter allXmlDataExporter; 
        public bool IsLoggedIn { get => isLoggedIn; }
        public string LoggedInUser { get => loggedInUser; }

        public EasterEggService(AllXmlDataExporter allXmlDataExporter)
        {
            this.allXmlDataExporter = allXmlDataExporter;
            LoadUsersFromFile();
        }

        public string HandleOption(int option, string username = "", string password = "")
        {
            switch (option)
            {
                case 1:
                    return Register(username, password) ? "Registration successful!" : "Username already exists.";
                case 2:
                    return Login(username, password) ? "Login successful!" : "Invalid username or password.";
                case 3:
                    isLoggedIn = false;
                    return "Logged out successfully.";
                default:
                    return "Invalid option.";
            }
        }
        
        public void ExportDatabaseToXml()
        {
            allXmlDataExporter.ExportDatabaseToXml();
        }

        public string DisplayAsciiArt()
        {
            return @"
                               /\_/\
                              ( o.o )
                               > ^ <
                            ";
        }

        bool Register(string username, string password)
        {
            if (userDatabase.Exists(user => user.Username == username)) return false;

            userDatabase.Add(new User { Username = username, Password = password });
            SaveUsersToFile();
            return true;
        }

        bool Login(string username, string password)
        {
            var user = userDatabase.Find(u => u.Username == username);
            if (user != null && user.Password == password)
            {
                isLoggedIn = true;
                loggedInUser = username;
                return true;
            }
            return false;
        }

        void LoadUsersFromFile()
        {
            if (File.Exists(UserFilePath))
            {
                var json = File.ReadAllText(UserFilePath);
                userDatabase = JsonSerializer.Deserialize<List<User>>(json) ?? new();
            }
        }

        void SaveUsersToFile()
        {
            var json = JsonSerializer.Serialize(userDatabase);
            File.WriteAllText(UserFilePath, json);
        }
    }
}
