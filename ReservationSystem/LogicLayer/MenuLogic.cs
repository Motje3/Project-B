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
                try { Console.Clear(); } catch { }
                return false; // Exit the loop.
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }


    public static bool JoinTour(Visitor visitor)
    {

        List<Tour> allowedTours = Tour.PrintToursOpenToday();
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join\n");

        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= allowedTours.Count)
        {
            Tour chosenTour = allowedTours[tourNumber - 1];
            chosenTour.AddVisitor(visitor);
            try { Console.Clear(); } catch { }
            MenuPresentation.ShowFullMenu(visitor);
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
        _printTourString(Tour.FindTourById(visitor.AssingedTourId));

        List<Tour> allowedTours = Tour.PrintToursOpenToday();
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join");

        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= allowedTours.Count)
        {
            Tour chosenTour = allowedTours[tourNumber - 1];
            Tour visitorsTour = Tour.FindTourById(visitor.AssingedTourId);
            if (visitorsTour != null)
            {
                visitorsTour.TransferVisitor(visitor, chosenTour);
                try { Console.Clear(); } catch { }
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
        Tour visitorsTour = Tour.FindTourById(visitor.AssingedTourId);
        if (visitorsTour == null)
        {
            Console.WriteLine("Error: Unable to find the tour.");
            return true; // Continue showing the menu.
        }

        visitorsTour.RemoveVisitor(visitor);

        Console.WriteLine("\nReservation has been canceled successfully.");
        Thread.Sleep(1500 * 1);
        try { Console.Clear(); } catch { }

        return false; // Exit the loop.
    }


    public static void _printTourString(Tour tour)
    {
        if (tour == null) return;
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        string hour = tour.StartTime.Hour.ToString("D2");
        string minute = tour.StartTime.Minute.ToString("D2");

        Console.WriteLine($"{hour}:{minute} {tourDate} | Duration: {tour.Duration} minutes\n");
    }
}
