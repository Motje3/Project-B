using Newtonsoft.Json;

public class Ticket
{
    public string TicketCode { get; set; }
    public int NumberOfPeople { get; set; }
    public List<Visitor> Visitors { get; set; }
    public Visitor visitor { get; set; }


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

            // Additional validation logic could be added here
            return tickets;
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            Console.WriteLine($"An error occurred: {ex.Message}");
            return new List<Ticket>(); // Return an empty list or handle the error as needed
        }
    }
}
