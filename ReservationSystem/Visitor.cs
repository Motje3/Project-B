public class Visitor
{
    public Guid VisitorId { get; private set; }
    public string TicketCode { get; set; } // Add ticket code property
    public Guid? AssingedTourId { get; private set; }

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

    public static void ShowMyTourTimes(Visitor visitor)
    {
        if (visitor.AssingedTourId.HasValue)
        {
            // Find the tour this visitor is assigned to
            GuidedTour myTour = GuidedTour.CurrentTours.FirstOrDefault(t => t.TourId == visitor.AssingedTourId.Value);
            if (myTour != null)
            {
                Console.WriteLine($"Tour Start Time for {visitor.TicketCode}: {myTour.StartTime.ToString("g")}");
                Console.WriteLine($"Tour End Time for {visitor.TicketCode}: {myTour.EndTime.ToString("g")}");
            }
            else
            {
                Console.WriteLine("No tour found for this visitor.");
            }
        }
        else
        {
            Console.WriteLine("No assigned tour for this visitor.");
        }
    }

}
