using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class Ticket
{
    public string TicketCode { get; set; }
    public Visitor visitor { get; set; }  // Now only a single Visitor object per ticket

    public static List<string> Tickets { get; set; } = new List<string>();
    public static string OnlineTicketsFilePath = "./JSON-Files/OnlineTickets.json";

    static Ticket()
    {
        Tickets = LoadTicketsFromFile();
    }

    public static List<string> LoadTicketsFromFile()
    {
        try
        {
            if (!File.Exists(OnlineTicketsFilePath))
            {
                throw new FileNotFoundException($"The file {OnlineTicketsFilePath} was not found.");
            }

            string jsonContent = File.ReadAllText(OnlineTicketsFilePath);
            var tickets = JsonConvert.DeserializeObject<List<string>>(jsonContent);

            if (tickets == null) throw new Exception("Failed to deserialize the JSON content.");

            return tickets;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<string>(); // Return an empty list or handle the error as needed
        }
    }

    public static bool IsCodeValid(string code)
    {
        try
        {
            // Check if the list contains the given code
            return Tickets.Contains(code);
        }
        catch (Exception ex)
        {
            Console.WriteLine("An error occurred while checking the ticket code: " + ex.Message);
            // Optionally handle errors when checking for the code
            return false; // Or handle this scenario differently based on your application's needs
        }
    }
}
