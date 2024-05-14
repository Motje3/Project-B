namespace ReservationSystem;

public class TicketInputRL : View
{
    public static string Show()
    {
        WriteLine("Enter your unique ticket code:");
        return ReadLine();  // userCode
    }
}