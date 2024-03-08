public class Visitor
{
    public string Name { get; set; }
    public int TicketCount { get; set; }
    public Guid VisitorId { get; private set; }
    public List<Ticket> Tickets { get; set; } // Add this line

    public Visitor(string name, int ticketCount)
    {
        Name = name;
        TicketCount = ticketCount;
        VisitorId = Guid.NewGuid();
        Tickets = new List<Ticket>(); // Initialize the list
    }

    // Method to add tickets to the visitor
    public void AddTicket(Ticket ticket)
    {
        Tickets.Add(ticket);
    }
}
