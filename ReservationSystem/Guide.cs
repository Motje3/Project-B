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
        } else if (tour.ExpectedVisitors.Contains(visitor))
        {
            if (!tour.PresentVisitors.Contains(visitor))
            {
                NewTour.PresentVisitors.Add(visitor);
                GuidedTour.EditTourInJSON(tour, NewTour);
                return;
            }
        }
        else if (!tour.ExpectedVisitors.Contains(visitor))
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