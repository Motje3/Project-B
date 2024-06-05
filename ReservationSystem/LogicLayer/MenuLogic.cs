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
        bool tourJoined = false;

        while (!tourJoined)
        {
            // Display available tours
            if (!TourTools.ShowAvailableTours(visitor))
            {
                return false;
            }

            // Prompt the user to choose a tour
            string chosenTourNumber = PickTourRL.Show();

            // Early return for testing
            if (chosenTourNumber == "GETMEOUT")
                return false;

            // Get the list of available tours, ordered by StartTime
            List<Tour> availableTours = TourTools.TodaysTours
                .Where(tour => !tour.Started && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > Program.World.Now)
                .OrderBy(tour => tour.StartTime)
                .ToList();

            // Validate user input and attempt to join the selected tour
            if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= availableTours.Count)
            {
                Tour chosenTour = availableTours[tourNumber - 1];
                chosenTour.AddVisitor(visitor);
                TourDataManager.SaveTours();

                try { Console.Clear(); } catch { }

                JoinTourSuccesMessage.Show(chosenTour);
                Thread.Sleep(2000);
                JoinTourSuccesMessage.Show2();
                var key = Program.World.ReadKey(true).Key;

                while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar)
                {
                    key = Program.World.ReadKey(true).Key;
                }

                if (key == ConsoleKey.Spacebar)
                {
                    try { Console.Clear(); } catch { }
                    Reservation.ShowFullMenu(visitor);
                }
                else
                {
                    try { Console.Clear(); } catch { }
                }

                tourJoined = true; // Mark tour as joined successfully
            }
            else
            {
                try { Console.Clear(); } catch { }
                InvalidRL.Show("Please choose a valid tour number. The tour number is located left of the tour and coloured in blue. \n"); // "Invalid choice." + subMessage
            }
        }

        return true;
    }



    public static void ChangeTour(Visitor visitor)
    {
        // Show current reservation details
        string reservationDetails = Visitor.GetCurrentReservation(visitor);
        Program.World.WriteLine(reservationDetails);

        bool tourChanged = false;

        while (!tourChanged)
        {
            // Display available tours and allow user to choose
            TourTools.ShowAvailableTours(visitor);

            // Prompt the user to choose a tour
            string chosenTourNumber = ChangeTourRL.Show();
            // Early return for testing
            if (chosenTourNumber == "GETMEOUT" || chosenTourNumber.ToLower() == "q")
            {
                Reservation.ShowFullMenu(visitor);
                return;
            }


            // Get the list of available tours, ordered by StartTime and only future tours
            List<Tour> availableTours = TourTools.TodaysTours
                .Where(tour => !tour.Started && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > Program.World.Now && !tour.ExpectedVisitors.Any(v => v.VisitorId == visitor.VisitorId))
                .OrderBy(tour => tour.StartTime)
                .ToList();

            // Validate user input and attempt to change to the selected tour
            if (int.TryParse(chosenTourNumber, out int tourNumber) && tourNumber > 0 && tourNumber <= availableTours.Count)
            {
                Tour chosenTour = availableTours[tourNumber - 1];
                Tour visitorsTour = TourTools.FindTourByVisitorTicketCode(visitor.TicketCode);

                if (visitorsTour != null)
                {
                    visitorsTour.TransferVisitor(visitor, chosenTour);
                    try { Console.Clear(); } catch { }
                    ChangeTourSucces.Show(chosenTour);
                    Thread.Sleep(2000);
                    JoinTourSuccesMessage.Show2();

                    var key = Program.World.ReadKey(true).Key;

                    while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar)
                    {
                        key = Program.World.ReadKey(true).Key;
                    }

                    if (key == ConsoleKey.Spacebar)
                    {
                        try { Console.Clear(); } catch { }
                        Reservation.ShowFullMenu(visitor);
                    }
                    else
                    {
                        try { Console.Clear(); } catch { }
                    }

                    tourChanged = true; // Mark tour as changed successfully
                }
                else
                {
                    NoTourRegistrationMessage.Show();
                    break; // Exit the loop if no current tour registration is found
                }
            }
            else
            {
                try { Console.Clear(); } catch { }

                InvalidRL.Show("Please choose a valid tour number. The tour number is located left of the tour and coloured in blue. \n"); // "Invalid choice." + subMessage
            }
        }
    }




    public static bool CancelTour(Visitor visitor)
    {
        Tour visitorsTour = TourTools.FindTourByVisitorTicketCode(visitor.TicketCode);

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
        var key = Program.World.ReadKey(true).Key;

        while (key != ConsoleKey.Enter)
        {
            key = Program.World.ReadKey(true).Key;
        }

        if (key == ConsoleKey.Enter)
        {
            try { Console.Clear(); } catch { }
        }
        return false;
    }
}
