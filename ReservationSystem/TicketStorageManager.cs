using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class TicketStorageManager
{
    private string filePath;

    public TicketStorageManager(string filePath)
    {
        this.filePath = filePath;
    }

    public void SaveTicketInfo(Visitor visitor)
    {
        var ticketHolderInfo = new List<object>();

        if (File.Exists(filePath))
        {
            string existingData = File.ReadAllText(filePath);
            ticketHolderInfo = JsonConvert.DeserializeObject<List<object>>(existingData) ?? new List<object>();
        }

        foreach (var ticket in visitor.Tickets)
        {
            // Now also include the Time property of each ticket
            ticketHolderInfo.Add(new
            {
                VisitorName = visitor.Name,
                TicketCode = ticket.TicketCode,
                VisitorId = visitor.VisitorId,
                Time = ticket.Time,
                IsActive = ticket.IsActive

            });
        }

        // Serialize the list of tickets with holder names and times to JSON and save it to the file
        File.WriteAllText(filePath, JsonConvert.SerializeObject(ticketHolderInfo, Formatting.Indented));
    }


    // Optionally, create a method to read the ticket information back into the application
    public List<Ticket> LoadTicketInfo()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("File does not exist.");
            return new List<Ticket>();
        }
        string jsonData = File.ReadAllText(filePath);
        List<Ticket> ticketInfoList = JsonConvert.DeserializeObject<List<Ticket>>(jsonData) ?? new List<Ticket>();
        return ticketInfoList;
    }

    public void SaveTicketInfoList(List<Ticket> ticketInfoList)
    {
        string jsonData = JsonConvert.SerializeObject(ticketInfoList, Formatting.Indented);
        File.WriteAllText(filePath, jsonData);
    }
}
