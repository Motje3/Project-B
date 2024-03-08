public class Ticket
{
    public string TicketCode { get; private set; }
    public Guid VisitorId { get; private set; }

    public Ticket(Guid visitorId)
    {
        VisitorId = visitorId;
        TicketCode = GenerateRandomCode();
    }

    private string GenerateRandomCode()
    {
        // Example: Generate a simple random code, in real scenarios, ensure this meets your requirements
        return Guid.NewGuid().ToString().Substring(0, 8);
    }
}
