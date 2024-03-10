public class Ticket
{
    public string TicketCode { get; set; }
    public string Time { get; set; }
    public Guid VisitorId { get; set; }
    public bool IsActive { get; set; }

    public Ticket(Guid visitorId, string time)
    {
        VisitorId = visitorId;
        TicketCode = GenerateRandomCode();
        Time = time;
        IsActive = true;
    }

    private string GenerateRandomCode()
    {
        // Example: Generate a simple random code, in real scenarios, ensure this meets your requirements
        return Guid.NewGuid().ToString().Substring(0, 8);
    }
}
