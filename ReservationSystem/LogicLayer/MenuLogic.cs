public class MenuLogic
{
    public bool HandleRestrictedMenuChoice(string choice, Visitor visitor)
    {
        switch (choice)
        {
            case "1":
                GuidedTour.PrintToursOpenToday();
                JoinTour(visitor);
                return true;
            case "2":
                return false; // Exit the loop
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }

    public bool HandleFullMenuChoice(string choice, Visitor visitor)
    {
        switch (choice)
        {
            case "1":
                ChangeTour(visitor);
                return true;
            case "2":
                CancelTour(visitor);
                return true;
            case "3":
                return false; // Exit the loop
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }

    public static void JoinTour(Visitor visitor)
    {
        List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join");
        foreach (var tour in allowedTours)
        {
            Console.WriteLine($"{allowedTours.IndexOf(tour) + 1}. Tour at {tour.StartTime} with {tour.MaxCapacity - tour.ExpectedVisitors.Count} available spots.");
        }

        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= allowedTours.Count)
        {
            GuidedTour chosenTour = allowedTours[tourNumber - 1];
            chosenTour.AddVisitor(visitor);
            Console.WriteLine("\nYou have successfully joined the tour.");
        }
        else
        {
            Console.WriteLine("Invalid choice, please choose a valid tour number.");
        }
    }

    public static void ChangeTour(Visitor visitor)
    {
        Console.WriteLine("\nYour current tour reservation is:");
        _printTourString(GuidedTour.FindTourById(visitor.AssingedTourId));

        List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join");

        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= allowedTours.Count)
        {
            GuidedTour chosenTour = allowedTours[tourNumber - 1];
            GuidedTour visitorsTour = GuidedTour.FindTourById(visitor.AssingedTourId);
            if (visitorsTour != null)
            {
                visitorsTour.TransferVisitor(visitor, chosenTour);
                Console.WriteLine($"\nYou have successfully transferred to the new tour: {chosenTour.StartTime}.\n");
            }
            else
            {
                Console.WriteLine("\nYou are not currently registered on any tour.");
            }
        }
        else
        {
            Console.WriteLine("Invalid choice, please choose a number next to the tour you wish to join");
        }
    }

    private static void CancelTour(Visitor visitor)
    {
        GuidedTour visitorsTour = GuidedTour.FindTourById(visitor.AssingedTourId);
        if (visitorsTour == null)
        {
            Console.WriteLine("Error: Unable to find the tour.");
            return;
        }

        visitorsTour.RemoveVisitor(visitor);
        Console.WriteLine("Tour reservation canceled successfully.");
    }

    private static void _printTourString(GuidedTour tour)
    {
        if (tour == null) return;
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        string hour = tour.StartTime.Hour.ToString("D2");
        string minute = tour.StartTime.Minute.ToString("D2");

        Console.WriteLine($"{hour}:{minute} {tourDate} | Duration: {tour.Duration} minutes\n");
    }
}
