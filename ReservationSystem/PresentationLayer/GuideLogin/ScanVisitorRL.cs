namespace ReservationSystem;

public class ScanVisitor : View
{
    public static string Show()
    {
        Write("Scan visitor code: ");
        return ReadLine();  // ticketCode
    }
}