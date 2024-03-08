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
        // Prepare the data structure to be saved
        var ticketHolderInfo = new List<object>();
        
        // Check if the file already exists to append to it
        if (File.Exists(filePath))
        {
            string existingData = File.ReadAllText(filePath);
            ticketHolderInfo = JsonConvert.DeserializeObject<List<object>>(existingData) ?? new List<object>();
        }
        
        foreach (var ticket in visitor.Tickets)
        {
            ticketHolderInfo.Add(new { VisitorName = visitor.Name, TicketCode = ticket.TicketCode, VisitorId = visitor.VisitorId });
        }

        // Serialize the list of tickets with holder names to JSON and save it to the file
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
