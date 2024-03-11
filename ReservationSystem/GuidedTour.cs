using System.Text.RegularExpressions;
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

        Console.WriteLine("Enter the number of tickets you would like to purchase: (For people over 18 year old)");
        int ticketCount;
        while (!int.TryParse(Console.ReadLine(), out ticketCount) || ticketCount <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of tickets: ");
        }

        decimal pricePerTicket = 18; // $20 for adults, $10 for children
        decimal totalPrice = ticketCount * pricePerTicket;

        Console.WriteLine("Enter the time you would like to visit (between 09:00 AM and 17:00):");
        string time = Console.ReadLine();
        bool isValidTime = false;

        while (!isValidTime)
        {
            isValidTime = ValidateTimeFormat(time) && IsTimeInRange(time);

            if (!isValidTime)
            {
                Console.WriteLine("Invalid time. Please enter a time in HH:MM format, between 09:00 and 17:00:");
                time = Console.ReadLine(); // Update the time variable with new user input
            }
        }

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
    public bool ValidateTimeFormat(string time)
    {
        // Validate that time is in the correct HH:MM format
        return Regex.IsMatch(time, @"^(?:[01]\d|2[0-3]):[0-5]\d$");
    }

    public bool IsTimeInRange(string time)
    {
        // Validate that time is within the range 09:00 to 17:00
        TimeSpan startTime = TimeSpan.FromHours(9); // 09:00
        TimeSpan endTime = TimeSpan.FromHours(17); // 17:00
        TimeSpan inputTime;

        if (TimeSpan.TryParse(time, out inputTime))
        {
            return inputTime >= startTime && inputTime <= endTime;
        }
        return false;
    }
}
