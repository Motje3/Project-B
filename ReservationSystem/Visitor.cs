public class Visitor
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Guid VisitorId { get; private set; }
    public List<Ticket> Tickets { get; set; } // Add this line
    public int RondleidingChoice { get; set; }

    public Visitor(string name, int rondleidingChoice)
    {
        Name = name;
        VisitorId = Guid.NewGuid();
        Tickets = new List<Ticket>(); // Initialize the list
        RondleidingChoice = rondleidingChoice;
    }

    // Method to add tickets to the visitor
    public void AddTicket(Ticket ticket)
    {
        Tickets.Add(ticket);
    }
}
