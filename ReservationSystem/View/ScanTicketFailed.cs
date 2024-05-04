namespace ReservationSystem;

public class ScanTicketFailed : View
{
    public string Show()
    {
        WriteLine("\nThis visitor doesn't have reservation for this tour,");
        WriteLine("Try again or try another visitor");
        return ReadLine();  // return visitorTicket
    }
}