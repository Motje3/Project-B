public class Visitor
{
    public Guid VisitorId { get; private set; }
    public string TicketCode { get; set; } // Add ticket code property
    public Guid AssingedTourId { get; set; }

    public Visitor(string ticketCode)
    {
        VisitorId = Guid.NewGuid();
        TicketCode = ticketCode; // Store the ticket code
    }

    public bool HasReservation(Visitor visitor)
    {
        //updated to return true instade of false. no need for if statments. 
        //Return true if the visitor is in any tour, indicating they have a reservation
        return GuidedTour.CurrentTours.Any(tour => tour.ExpectedVisitors.Any(v => v.VisitorId == visitor.VisitorId));
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
                    continue; // Skip to the next visitor if the current one is null
                }
                if (currentVisitor.TicketCode == ticketCode)
                {
                    foundVisitor = currentVisitor;
                    break; // Exit the inner loop as we found the visitor
                }
            }
            if (foundVisitor != null)
                break;
        }

        return foundVisitor;
    }
}