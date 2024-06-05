using Newtonsoft.Json;
using ReservationSystem;

public static class Ticket
{
    public static List<string> Tickets { get; set; } = new List<string>();
    public static string OnlineTicketsFilePath = "./JSON-Files/OnlineTickets.json"; // Adjust the path as necessary

    static Ticket()
    {
        LoadTickets();
    }

    public static void LoadTickets()
    {
        try
        {
            // Read the JSON content from the file
            string jsonContent = Program.World.ReadAllText(OnlineTicketsFilePath);

            // Deserialize the JSON content into the Tickets list
            Tickets = JsonConvert.DeserializeObject<List<string>>(jsonContent);
        }
        catch (FileNotFoundException fnfe)
        {
            Program.World.WriteLine("File not found: " + fnfe.Message);
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
            Program.World.WriteLine("An error occurred while checking the ticket code: " + ex.Message);
            return false;
        }
    }
}
