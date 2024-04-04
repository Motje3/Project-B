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
                    int numberOfPeople = ticket.NumberOfPeople;

                    _guidedTour.ListAvailableTours(numberOfPeople);
                    Console.WriteLine($"\nPlease enter the hour of the tour you wish to join ({_guidedTour.StartTime} to {_guidedTour.EndTime}):");

                    if (int.TryParse(Console.ReadLine(), out int chosenHour))
                    {
                        if (ticket == null)
                        {
                            Console.WriteLine("Ticket code is not valid.");
                            break; // Exit the switch case
                        }

                        foreach (var visitorInfo in ticket.Visitors)
                        {
                            Visitor visitor = new Visitor(visitorInfo.Name, chosenHour, ticketCode); // Create a Visitor object for each person in the ticket.

                            if (!_guidedTour.JoinTour(chosenHour, visitor))
                            {
                                Console.WriteLine($"Failed to join {visitor.Name} to the tour. Please try again later.");
                                break; // If one fails, we exit the loop (consider how you want to handle partial success)
                            }
                            else
                            {
                                Console.WriteLine($"{visitor.Name} has successfully joined the {chosenHour}:00 tour.");
                                _reservationManager.SaveReservation(visitor);
                            }
                        }

                        _guidedTour.SaveGuidedToursToFile();
                        loopOption = false;  // Exit the main menu loop after successful operation.
                    }
                    else
                    {
                        Console.WriteLine("Invalid input. Please enter a valid tour hour.");
                    }
                    break;
                case "2":
                    loopOption = false;  // Exit the main menu loop.
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
                        int numberOfPeople = ticket.NumberOfPeople;

                        _guidedTour.ListAvailableTours(numberOfPeople);
                        Console.WriteLine($"\nPlease enter the hour of the tour you wish to join ({_guidedTour.StartTime} to {_guidedTour.EndTime}):");
                        if (int.TryParse(Console.ReadLine(), out int newTourHour))
                        {
                            if (_reservationManager.EditReservation(ticketCode, newTourHour))
                            {
                                selectingTour = false; // Exit the tour selection loop.
                            }
                            else
                            {
                                Console.WriteLine("Failed to update the guided tour. Please try a different hour or cancel.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Invalid input. Please enter a valid hour.");
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