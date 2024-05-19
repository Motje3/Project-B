using System.Net;
using ReservationSystem;

public class MenuLogic
{
    public bool HandleFullMenuChoice(string choice, Visitor visitor)
    {
        switch (choice)
        {
            case "1":
                ChangeTour(visitor);
                return false;
            case "2":
                return CancelTour(visitor); // Use the return value to determine whether to exit.
            case "3":
                try { Console.Clear(); } catch { }
                return false; // Exit the loop.
            default:
                InvalidRL.Show();
                return true;
        }
    }


    public static bool JoinTour(Visitor visitor)
    {
        // Display available tours
        Tour.ShowAvailableTours();


        // Prompt the user to choose a tour
        string chosenTourNumber = PickTourRL.Show();

        // Get the list of available tours, ordered by StartTime
        List<Tour> availableTours = Tour.TodaysTours
            .Where(tour => !tour.Completed && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > DateTime.Now)
            .OrderBy(tour => tour.StartTime)
            .ToList();


        // Validate user input and attempt to join the selected tour
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= availableTours.Count)
        {

            Tour chosenTour = availableTours[tourNumber - 1];
            chosenTour.AddVisitor(visitor);
            Tour.SaveTours();

            try { Console.Clear(); } catch { }

            JoinTourSuccesMessage.Show(chosenTour);
            Thread.Sleep(2000);
            JoinTourSuccesMessage.Show2();
            var key = Console.ReadKey(true).Key;

            while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar)
            {
                key = Console.ReadKey(true).Key;
            }

            if (key == ConsoleKey.Spacebar)
            {
                Console.Clear();
                Reservation.ShowFullMenu(visitor);
            }
            else
            {
                try { Console.Clear(); } catch { }
            }

            return true;
        }
        else
        {
            InvalidRL.Show("Please choose a valid tour number.\n"); // "Invalid choice." + subMessage
            return false; // Return false to indicate failure to join a tour
        }
    }


    public static void ChangeTour(Visitor visitor)
    {
        // Show current reservation details
        string reservationDetails = Visitor.GetCurrentReservation(visitor);
        Program.World.WriteLine(reservationDetails);

        // Display available tours and allow user to choose
        Tour.ShowAvailableTours();

        // Prompt the user to choose a tour
        string chosenTourNumber = ChangeTourRL.Show();

        // Get the list of available tours, ordered by StartTime and only future tours
        List<Tour> availableTours = Tour.TodaysTours
            .Where(tour => !tour.Completed && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > DateTime.Now)
            .OrderBy(tour => tour.StartTime)
            .ToList();

        // Validate user input and attempt to change to the selected tour
        if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= availableTours.Count)
        {
            Tour chosenTour = availableTours[tourNumber - 1];
            Tour visitorsTour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);

            if (visitorsTour != null)
            {
                visitorsTour.TransferVisitor(visitor, chosenTour);
                try { Console.Clear(); } catch { }
                ChangeTourSucces.Show(chosenTour);
                Thread.Sleep(2000);
                JoinTourSuccesMessage.Show2();

                var key = Console.ReadKey(true).Key;

                while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar)
                {
                    key = Console.ReadKey(true).Key;
                }

                if (key == ConsoleKey.Spacebar)
                {
                    Console.Clear();
                    Reservation.ShowFullMenu(visitor);
                }
                else
                {
                    try { Console.Clear(); } catch { }
                }
            }
            else
            {
                NoTourRegistrationMessage.Show();
            }
        }
        else
        {
            InvalidRL.Show("Please choose a number next to the tour you wish to join."); // "Invalid choice." + subMessage
        }
    }



    public static bool CancelTour(Visitor visitor)
    {
        Tour visitorsTour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);

        if (visitorsTour == null)
        {
            ErrorTourNotFound.Show();
            return true;
        }

        visitorsTour.RemoveVisitor(visitor);
        try { Console.Clear(); } catch { }
        ReservationCancelSucces.Show();
        Thread.Sleep(2000);

        JoinTourSuccesMessage.Show3();
        var key = Console.ReadKey(true).Key;

        while (key != ConsoleKey.Enter)
        {
            key = Console.ReadKey(true).Key;
        }

        if (key == ConsoleKey.Enter)
        {
            try { Console.Clear(); } catch { }
        }
        return false;
    } 
}
