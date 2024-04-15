using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

public class Ticket
{
    public string TicketCode { get; set; }
    public Visitor visitor { get; set; }  // Now only a single Visitor object per ticket

    public static List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public static string OnlineTicketsFilePath = "./JSON-Files/OnlineTickets.json";

    static Ticket()
    {
        Tickets = LoadTicketsFromFile();
    }

    public static List<Ticket> LoadTicketsFromFile()
    {
        try
        {
            if (!File.Exists(OnlineTicketsFilePath))
            {
                throw new FileNotFoundException($"The file {OnlineTicketsFilePath} was not found.");
            }

            string jsonContent = File.ReadAllText(OnlineTicketsFilePath);
            var tickets = JsonConvert.DeserializeObject<List<Ticket>>(jsonContent);

            if (tickets == null) throw new Exception("Failed to deserialize the JSON content.");

            return tickets;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<Ticket>(); // Return an empty list or handle the error as needed
        }
    }
}
