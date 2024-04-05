public class Visitor
{
    public string Name { get; set; }
    public Guid VisitorId { get; private set; }
    public DateTime TourTime { get; set; } // Use DateTime to represent the tour time
    public string TicketCode { get; set; } // Add ticket code property

    public Visitor(string name, DateTime tourTime, string ticketCode)
    {
        Name = name;
        VisitorId = Guid.NewGuid();
        TourTime = tourTime; // Store the tour time as DateTime
        TicketCode = ticketCode; // Store the ticket code
    }
}
