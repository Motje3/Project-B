public class Guide : Visitor
{
    public Guide(string ticketcode, Guid tourId) : base(ticketcode) 
    { /* Name , TicketCode */ 
        GuidedTour myTour = GuidedTour.FindTourById(tourId);
        base.AssingedTourId = tourId;
    }

    public void CheckInVisitor(Visitor visitor)
    {
        GuidedTour tour = GuidedTour.FindTourById(base.AssingedTourId);
        GuidedTour NewTour = tour.Clone();  // this will overwrite the old tour, Clone prefence changes to oldtour.

        if (tour == null)
        {
            Console.WriteLine("\nNo matching tour not found");
        }

        bool ExpectedVisitorsContainsParameter = false;
        foreach (Visitor currencVisitor in tour.ExpectedVisitors)
        {
            if (currencVisitor.TicketCode == visitor.TicketCode)
            {
                ExpectedVisitorsContainsParameter = true;
            }
        }
        bool PresentVisitorsContainsParameter = false;
        foreach (Visitor currencVisitor in tour.PresentVisitors)
        {
            if (currencVisitor.TicketCode == visitor.TicketCode)
            {
                PresentVisitorsContainsParameter = true;
            }
        }

        // 
        if (ExpectedVisitorsContainsParameter)
        {
            if (!PresentVisitorsContainsParameter)
            {
                NewTour.PresentVisitors.Add(visitor);
                GuidedTour.EditTourInJSON(tour, NewTour);    
                return;
            }
        }
        else
        {
            // fail save message
            Console.WriteLine("\nNo matching tour not found");
        }       
    }  
    public void CompleteTour() 
    {
        var newTour = GuidedTour.FindTourById(base.AssingedTourId);
        var oldTour = GuidedTour.FindTourById(base.AssingedTourId);
        if (base.AssingedTourId == null)
        {
            return;
        }
        if (newTour == null)
            return;
        if (newTour.Deleted == true)
        {
            return;
        }
        newTour.Completed = true;
        GuidedTour.EditTourInJSON(oldTour, newTour);
    }
}