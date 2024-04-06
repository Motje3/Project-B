using System.Globalization;

public class MenuManager
{
    private ReservationManager _reservationManager;
    private GuidedTour _guidedTour;

    public MenuManager(ReservationManager reservationManager, GuidedTour guidedTour)
    {
        _reservationManager = reservationManager;
        _guidedTour = guidedTour;
    }

    public void ShowRestrictedMenu(string ticketCode)
    {
        bool loopOption = true;
        while (loopOption)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Join a tour");
            Console.WriteLine("2. Exit");
            Console.Write("\nEnter your choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                
                    var ticket = _reservationManager.Tickets.FirstOrDefault(t => t.TicketCode == ticketCode);
                    if (ticket == null)
                    {
                        Console.WriteLine("Ticket code is not valid.");
                        break; // Exit the switch case
                    }

                    int numberOfPeople = ticket.NumberOfPeople;
                    _guidedTour.ListAvailableTours(numberOfPeople);


                    Console.WriteLine($"\nPlease enter the time of the tour you wish to join (e.g., '1:00 PM') between {_guidedTour.StartTime.ToString("h:mm tt")} to {_guidedTour.EndTime.ToString("h:mm tt")}:");

                    

                    string inputTime = Console.ReadLine();
                    if (DateTime.TryParseExact(inputTime, new[] { "h:mm tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime chosenTourTime))
                    {
                        chosenTourTime = DateTime.Today.AddHours(chosenTourTime.Hour).AddMinutes(chosenTourTime.Minute);

                        if (chosenTourTime < _guidedTour.StartTime || chosenTourTime > _guidedTour.EndTime)
                        {
                            Console.WriteLine("The chosen time is outside the tour operation hours!!!");
                            break;
                        }

                        bool joinSuccess = true;
                        foreach (var visitorInfo in ticket.Visitors)
                        {
                            Visitor visitor = new Visitor(visitorInfo.Name, chosenTourTime, ticketCode); // Create a Visitor object for each person in the ticket.

                            if (!_guidedTour.JoinTour(chosenTourTime, visitor))
                            {
                                Console.WriteLine($"Failed to join {visitor.Name} to the tour. Please try again later.");
                                joinSuccess = false;
                                break; // If one fails, we exit the loop (consider how you want to handle partial success)
                            }
                        }

                        if (joinSuccess)
                        {
                            foreach (var visitorInfo in ticket.Visitors)
                            {
                                Visitor visitor = new Visitor(visitorInfo.Name, chosenTourTime, ticketCode);
                                _reservationManager.SaveReservation(visitor);
                            }
                            _guidedTour.SaveGuidedToursToFile();
                            loopOption = false;  // Exit the main menu loop after successful operation.
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter the tour time in the format 'H:MM AM' or 'H:MM PM'.");
                    }
                    break;

                case "2":
                    loopOption = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }


    public void ShowFullMenu(string ticketCode)
    {
        bool choosingOption = true;
        while (choosingOption)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Join a different tour");
            Console.WriteLine("2. Cancel my tour reservation");
            Console.WriteLine("3. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    bool selectingTour = true;
                    while (selectingTour)
                    {
                        var ticket = _reservationManager.Tickets.FirstOrDefault(t => t.TicketCode == ticketCode);
                        if (ticket == null)
                        {
                            Console.WriteLine("Ticket code is not valid.");
                            break; // Exit the switch case and the while loop
                        }

                        int numberOfPeople = ticket.NumberOfPeople;

                        _guidedTour.ListAvailableTours(numberOfPeople);
                        Console.WriteLine($"\nPlease enter the time of the tour you wish to join (e.g., '1:00 PM') between {_guidedTour.StartTime.ToString("h:mm tt")} to {_guidedTour.EndTime.ToString("h:mm tt")}:");

                        string inputTime = Console.ReadLine();
                        if (DateTime.TryParseExact(inputTime, new[] { "h:mm tt" }, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime newTourDateTime))
                        {
                            newTourDateTime = DateTime.Today.AddHours(newTourDateTime.Hour).AddMinutes(newTourDateTime.Minute);

                            if (newTourDateTime < _guidedTour.StartTime || newTourDateTime > _guidedTour.EndTime)
                            {
                                Console.WriteLine("The chosen time is outside the tour operation hours!!");
                                continue; // Ask for input again
                            }

                            if (_reservationManager.EditReservation(ticketCode, newTourDateTime))
                            {
                                selectingTour = false; // Exit the tour selection loop.
                            }
                            else
                            {
                                Console.WriteLine("Failed to update the tour time. Please try a different time or cancel.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter the tour time in the format 'H:MM AM' or 'H:MM PM'.");
                        }
                    }
                    break;

                case "2":
                    _reservationManager.CancelReservation(ticketCode);
                    choosingOption = false; // Exit the main menu loop after cancelling.
                    break;
                case "3":
                    choosingOption = false; // Exit the main menu loop.
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }


}