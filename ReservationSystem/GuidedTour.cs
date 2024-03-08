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

    public void PromptForReservation()
    {
        Console.WriteLine("Enter your name:");
        string name = Console.ReadLine();

        Console.WriteLine("Enter the number of tickets:");
        int ticketCount;
        while (!int.TryParse(Console.ReadLine(), out ticketCount) || ticketCount <= 0)
        {
            Console.WriteLine("Invalid input. Please enter a valid number of tickets:");
        }

        Visitor visitor = new Visitor(name, ticketCount);
        ReserveSpot(visitor);
    }

    private void ReserveSpot(Visitor visitor)
    {
        if (TryReserveSpot(visitor))
        {
            Console.WriteLine($"Reservation confirmed for {visitor.Name} with {visitor.TicketCount} tickets.");
        }
        else
        {
            Console.WriteLine("Sorry, there are not enough spots available for your reservation.");
        }
    }
}
