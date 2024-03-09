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
                Time = ticket.Time // Include the ticket time here
            });
        }

        // Serialize the list of tickets with holder names and times to JSON and save it to the file
        File.WriteAllText(filePath, JsonConvert.SerializeObject(ticketHolderInfo, Formatting.Indented));
    }


    // Optionally, create a method to read the ticket information back into the application
    public List<object> LoadTicketInfo()
    {
        if (!File.Exists(filePath)) return new List<object>();

        string jsonData = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<List<object>>(jsonData) ?? new List<object>();
    }
}
