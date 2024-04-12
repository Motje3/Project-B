public class Visitor
{
    public string Name { get; set; }
    public Guid VisitorId { get; private set; }

    public string TicketCode { get; set; } // Add ticket code property

    public Visitor(string name, string ticketCode)
    {
        Name = name;
        VisitorId = Guid.NewGuid();
        TicketCode = ticketCode; // Store the ticket code
    }

    public bool HasReservation()
    {
        if (GuidedTour.CheckIfVisitorInTour(this))
        {
            return false;
        }
        return true;
    }

    public static Visitor FindVisitorByTicketCode(string ticketCode)
    {
        Visitor foundVisitor = null;
        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            foreach(Visitor currentVisitor in currentTour.ExpectedVisitors)
            {
                if (currentVisitor.TicketCode == ticketCode)
                {
                    foundVisitor = currentVisitor;
                    break;
                }
            }
            if (foundVisitor != null)
                break;
        }

        return foundVisitor;
    }

    
}
