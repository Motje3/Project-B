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


        Visitor visitor = new Visitor(name, ticketCount);
        ReserveSpot(visitor);
        Console.WriteLine($"Total price for {ticketCount} ticket(s): ${totalPrice}");
        return visitor; // This will now compile successfully with the method return type changed.
    }


    private void ReserveSpot(Visitor visitor)
    {
        if (TryReserveSpot(visitor))
        {
            for (int i = 0; i < visitor.TicketCount; i++)
            {
                // Generate a new ticket for each count and add it to the visitor
                Ticket newTicket = new Ticket(visitor.VisitorId);
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
