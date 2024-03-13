public class Ticket
{
    public string TicketCode { get; set; }
    public string Time { get; set; }
    public Guid VisitorId { get; set; }
    public bool IsActive { get; set; }

    public Ticket(Guid visitorId, string time)
    {
        VisitorId = visitorId;
        Time = time;
        IsActive = true;
    }
}
