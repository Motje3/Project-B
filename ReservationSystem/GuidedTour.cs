public class GuidedTour
{
    private const int MaxCapacity = 13;
    private int currentCapacity;
    private List<Visitor> reservations;

    public GuidedTour()
    {
        currentCapacity = 0;
        reservations = new List<Visitor>();
    }

    public bool TryReserveSpot(Visitor visitor)
    {
        if (currentCapacity + visitor.TicketCount <= MaxCapacity)
        {
            reservations.Add(visitor);
            currentCapacity += visitor.TicketCount;
            return true;
        }
        else
        {
            return false;
        }
    }

    public Visitor PromptForReservation()
    {
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter the number of tickets: (For people over 18 year old)");
        int ticketCount;
        while (!int.TryParse(Console.ReadLine(), out ticketCount) || ticketCount <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of tickets:");
        }

        decimal pricePerTicket = 18; // $20 for adults, $10 for children
        decimal totalPrice = ticketCount * pricePerTicket;

        Console.WriteLine("Enter the time you would like to visit (between 9 AM and 5 PM):");
        string time = Console.ReadLine();
        bool isValidTime = false;
        do
        {
  
            // Validate the time format and range. Simplified for illustration. Consider using DateTime.TryParse for real scenarios.
            isValidTime = time.CompareTo("09:00") >= 0 && time.CompareTo("17:00") <= 0;
            if (!isValidTime) Console.WriteLine("Invalid time. Please enter a time between 9 AM and 5 PM:");
        } while (!isValidTime);

        Visitor visitor = new Visitor(name, ticketCount);
        ReserveSpot(visitor, time);
        Console.WriteLine($"Total price for {ticketCount} ticket(s): ${totalPrice}");
        return visitor; // This will now compile successfully with the method return type changed.
    }


    private void ReserveSpot(Visitor visitor, string time)
    {
        if (TryReserveSpot(visitor))
        {
            for (int i = 0; i < visitor.TicketCount; i++)
            {
                // Generate a new ticket for each count and add it to the visitor
                Ticket newTicket = new Ticket(visitor.VisitorId, time);
                visitor.AddTicket(newTicket);
            }
            Console.WriteLine($"Reservation confirmed for {visitor.Name} with {visitor.TicketCount} tickets. Ticket codes are:");
            // Print each ticket code
            foreach (var ticket in visitor.Tickets)
            {
                Console.WriteLine(ticket.TicketCode);
            }
        }
        else
        {
            Console.WriteLine("Sorry, there are not enough spots available for your reservation.");
        }
    }
}
