public class Visitor
{
    public Guid VisitorId { get; private set; }
    public string TicketCode { get; set; } // Add ticket code property
    public Guid AssingedTourId { get;  set; }

    public Visitor(string ticketCode)
    {
        VisitorId = Guid.NewGuid();
        TicketCode = ticketCode; // Store the ticket code
    }

    public bool HasReservation()
    {
        //updated to return true instade of false. no need for if statments. 
        //Return true if the visitor is in any tour, indicating they have a reservation
        return GuidedTour.CheckIfVisitorInTour(this);
    }

    //probably not needed as we can just use hasreservation to check if vistor has reservation. 
    public static Visitor FindVisitorByTicketCode(string ticketCode)
    {
        Visitor foundVisitor = null;
        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            foreach (Visitor currentVisitor in currentTour.ExpectedVisitors)
            {
                if (currentVisitor == null)
                {
                    
                }
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
