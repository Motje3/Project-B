using System.Net;

public class MenuLogic
{
    public bool HandleRestrictedMenuChoice(string choice, Visitor visitor)
    {
        switch (choice)
        {
            case "1":
                return JoinTour(visitor);
                
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
                return CancelTour(visitor); // Use the return value to determine whether to exit.
            case "3":
                return false; // Exit the loop.
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }


    public static bool JoinTour(Visitor visitor)
    {

        List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join\n");
        
        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= allowedTours.Count)
        {
            GuidedTour chosenTour = allowedTours[tourNumber - 1];
            chosenTour.AddVisitor(visitor);
            Console.WriteLine("\nYou have successfully joined the chosen tour.\n");
            return false;
        }
        else
        {
            //need to change this into a do while loop so user is forced to enter a valid tour number.
            Console.WriteLine("Invalid choice, please choose a valid tour number.\n");
            return true;
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

    private static bool CancelTour(Visitor visitor)
    {
        GuidedTour visitorsTour = GuidedTour.FindTourById(visitor.AssingedTourId);
        if (visitorsTour == null)
        {
            Console.WriteLine("Error: Unable to find the tour.");
            return true; // Continue showing the menu.
        }

        visitorsTour.RemoveVisitor(visitor);
        Console.WriteLine("Tour reservation canceled successfully.");
        return false; // Exit the loop.
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
