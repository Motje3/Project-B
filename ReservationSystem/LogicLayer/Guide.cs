public class Guide : Visitor
{
    public Guide(string ticketcode, Guid tourId) : base(ticketcode)
    { /* Name , TicketCode */
        GuidedTour myTour = GuidedTour.FindTourById(tourId);
        base.AssingedTourId = tourId;
    }

    public void CheckInVisitor(Visitor visitor)
    {
        // Retrieve the tour using the visitor's assigned tour ID
        GuidedTour tour = GuidedTour.FindTourById(base.AssingedTourId);

        if (tour == null)
        {
            Console.WriteLine("\nNo matching tour found.");
            return;
        }

        GuidedTour newTour = tour.Clone();  // Clone the tour to create a modifiable copy

        // Check if visitor is expected and not yet present
        bool isExpected = tour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode);
        bool isPresent = tour.PresentVisitors.Any(v => v.TicketCode == visitor.TicketCode);

        if (isExpected && !isPresent)
        {
            newTour.PresentVisitors.Add(visitor);  // Add visitor to the present list
            JsonHelper.EditTour(newTour, tour.TourId, GuidedTour.JsonFilePath);  // Update the tour in JSON
            Console.WriteLine("\nVisitor checked in successfully.");
        }
        else if (!isExpected)
        {
            Console.WriteLine("\nVisitor not expected on this tour.");
        }
        else if (isPresent)
        {
            Console.WriteLine("\nVisitor already checked in.");
        }
    }


    public void CompleteTour()
    {
        if (base.AssingedTourId == null)
        {
            Console.WriteLine("No assigned tour ID.");
            return;
        }

        GuidedTour tour = GuidedTour.FindTourById(base.AssingedTourId);
        if (tour == null)
        {
            Console.WriteLine("Tour not found.");
            return;
        }

        if (tour.Deleted)
        {
            Console.WriteLine("Cannot complete a deleted tour.");
            return;
        }

        if (tour.Completed)
        {
            Console.WriteLine("Tour already completed.");
            return;
        }

        tour.Completed = true;
        // Assuming you have refactored JsonHelper.EditTour to handle updates
        JsonHelper.EditTour(tour, tour.TourId, GuidedTour.JsonFilePath);
        Console.WriteLine("Tour marked as completed.");
    }

}