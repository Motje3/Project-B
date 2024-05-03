using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Authenticator
{
    private class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static bool AuthenticateUser(string username, string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Username == username && cred.Password == password);
    }

    private static List<Credential> LoadUserCredentials()
    {
        string filePath = "./DataLayer/JSON-Files/AdminCredentials.json";
        if (File.Exists(filePath))
        {
            return JsonHelper.LoadFromJson<List<Credential>>(filePath);
        }
        else
        {
            Console.WriteLine("Credential file not found.");
            return new List<Credential>();
        }
    }
}
