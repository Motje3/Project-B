namespace ReservationSystem;

public class ScanTicket : View
{
    public string Show()
    {
        WriteLine("Please scan the ticket of a visitor or type in \"stop\" to stop checking in visitors");
        return ReadLine();  // return visitorTicket
    }
}


