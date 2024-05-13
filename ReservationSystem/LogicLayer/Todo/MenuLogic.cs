using System.Net;

public class MenuLogic
{



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
        // Display available tours
        Tour.ShowAvailableTours();

        // Prompt the user to choose a tour
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join:");
        string chosenTourNumber = Console.ReadLine();
        List<Tour> availableTours = Tour.TodaysTours.Where(tour => !tour.Completed && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity).ToList();

        // Validate user input and attempt to join the selected tour
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= availableTours.Count)
        {
            Tour chosenTour = availableTours[tourNumber - 1];
            chosenTour.AddVisitor(visitor);
            Tour.SaveTours();
            try { Console.Clear(); } catch { }
            Console.WriteLine("Tour joined successfully!");
            Thread.Sleep(2000);
            try { Console.Clear(); } catch { }
            return true;
        }
        else
        {
            Console.WriteLine("Invalid choice, please choose a valid tour number.\n");
            return false; // Return false to indicate failure to join a tour
        }
    }

    public static void ChangeTour(Visitor visitor)
    {
        // Show current reservation details
        string reservationDetails = Visitor.GetCurrentReservation(visitor);
        Console.WriteLine(reservationDetails);
        
        // Display available tours and allow user to choose
        Tour.ShowAvailableTours();
        Console.WriteLine("\nPlease choose a number next to the new tour you wish to join");

        string chosenTourNumber = Console.ReadLine();
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= Tour.TodaysTours.Count)
        {
            Tour chosenTour = Tour.TodaysTours[tourNumber - 1];
            Tour visitorsTour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);
            if (visitorsTour != null)
            {
                visitorsTour.TransferVisitor(visitor, chosenTour);
                try { Console.Clear(); } catch { }
                Console.WriteLine($"\nYou have successfully transferred to the new tour: {chosenTour.StartTime.ToString("h:mm tt")}.\n");
                Thread.Sleep(2000);
                try { Console.Clear(); } catch { }

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


    public static bool CancelTour(Visitor visitor)
    {
        Tour visitorsTour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);

        if (visitorsTour == null)
        {
            Console.WriteLine("Error: Unable to find the tour.");
            return true;
        }

        visitorsTour.RemoveVisitor(visitor);
        try { Console.Clear(); } catch { }
        Console.WriteLine("\nReservation has been canceled successfully.");
        Thread.Sleep(2000);
        try { Console.Clear(); } catch { }

        return false;
    }
}
