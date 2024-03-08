public class Visitor
{
    public string Name { get; set; }
    public int TicketCount { get; set; }
    public Guid VisitorId { get; private set; }

    public Visitor(string name, int ticketCount)
    {
        Name = name;
        TicketCount = ticketCount;
        VisitorId = Guid.NewGuid(); // Assign a unique ID to each visitor
    }
}
