namespace ReservationSystem;

public class TicketInput : View
{
    public static string Show()
    {
        WriteLine("Enter your unique ticket code:");
        return ReadLine();  // userCode
    }
}