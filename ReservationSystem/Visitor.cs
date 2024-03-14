public class Visitor
{
    public string Name { get; set; }
    public Guid VisitorId { get; private set; }
    public int RondleidingChoice { get; set; }
    public string TicketCode { get; set; } // Add ticket code property

    public Visitor(string name, int rondleidingChoice, string ticketCode)
    {
        Name = name;
        VisitorId = Guid.NewGuid();
        RondleidingChoice = rondleidingChoice;
        TicketCode = ticketCode; // Store the ticket code
    }
}