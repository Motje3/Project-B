using System;

namespace YourNamespace
{
    public static class JsonPaths
    {
        public static string AdminCredentialsFilePath = "./JSON-Files/AdminCredentials.json";
        public static string OnlineTicketsFilePath = "./JSON-Files/OnlineTickets.json";
        public static string ToursJsonFilePath => $"./JSON-Files/Tours-{DateTime.Today:yyyyMMdd}.json";
        public static string TourSettingsFilePath = "./JSON-Files/TourSettings.json";
        public static string GuideAssignmentsFilePath = "./JSON-Files/GuideAssignments.json";
        public static string GidsCredentialsFilePath = "./JSON-Files/GidsCredentials.json";
    }
}
