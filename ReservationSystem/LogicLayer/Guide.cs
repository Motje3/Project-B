public class Guide
{
    public Guid GuideId { get; private set; }
    public string Name { get; set; }
    public Guid AssignedTourId { get; set; }

    public Guide(string name, Guid tourId)
    {
        GuideId = Guid.NewGuid();
        Name = name;
        AssignedTourId = tourId;
    }

    public void CheckInVisitor(Visitor visitor)
    {
        // Retrieve the tour using the visitor's assigned tour ID
        Tour Mytour = Tour.FindTourById(this.AssignedTourId);

        if (Mytour == null)
        {
            Console.WriteLine("\nNo matching tour found.");
            return;
        }

        Tour newTour = Mytour.Clone();  // Clone the tour to create a modifiable copy

        // Check if visitor is expected and not yet present
        bool isExpected = Mytour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode);
        bool isPresent = Mytour.PresentVisitors.Any(v => v.TicketCode == visitor.TicketCode);

        if (isExpected && !isPresent)
        {
            newTour.PresentVisitors.Add(visitor);  // Add visitor to the present list
            JsonHelper.EditTour(newTour, Mytour.TourId, Tour.JsonFilePath);  // Update the tour in JSON
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
        if (AssignedTourId == null)
        {
            Console.WriteLine("No assigned tour ID.");
            return;
        }

        Tour tour = Tour.FindTourById(this.AssignedTourId);
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
        JsonHelper.EditTour(tour, tour.TourId, Tour.JsonFilePath);
        Console.WriteLine("Tour marked as completed.");
    }

}