using Newtonsoft.Json;

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
            string jsonContent = File.ReadAllText(OnlineTicketsFilePath);

            // Deserialize the JSON content into the Tickets list
            Tickets = JsonConvert.DeserializeObject<List<string>>(jsonContent);
        }
        catch (FileNotFoundException fnfe)
        {
            Console.WriteLine("File not found: " + fnfe.Message);
            // Handle the case where the file is not found - e.g., log the error, alert the user, etc.
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
