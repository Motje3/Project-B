namespace ReservationSystem;

public class InvalidTicketCode : View
{
    public static void Show(string ticketCode)
    {
        WriteLine($"\nThe code ['{ticketCode}'] is invalid");
        WriteLine("please provide a valid visitor ticketCode");
    }
}