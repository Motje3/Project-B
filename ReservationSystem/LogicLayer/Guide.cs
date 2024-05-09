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
        Tour myTour = Tour.FindTourById(this.AssignedTourId);

        if (myTour == null)
        {
            Console.WriteLine("\nNo matching tour found.");
            return;
        }

        // Check if the visitor is expected and not yet checked in
        if (!myTour.ExpectedVisitors.Contains(visitor))
        {
            Console.WriteLine("\nVisitor not expected on this tour.");
            return;
        }

        if (myTour.PresentVisitors.Contains(visitor))
        {
            Console.WriteLine("\nVisitor already checked in.");
            return;
        }

        myTour.PresentVisitors.Add(visitor);
        Tour.SaveTours(); // Assuming there's a SaveTours method handling all tour updates
        Console.WriteLine("\nVisitor checked in successfully.");
    }

    public void CompleteTour()
    {
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
        Tour.SaveTours(); // Save all tours, reflecting the completed state
        Console.WriteLine("Tour marked as completed.");
    }
}
