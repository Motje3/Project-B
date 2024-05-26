namespace ReservationSystem;

public class ScanVisitor : View
{
    public static string Show()
    {
        Write("\nScan visitor code: ");
        return ReadLine();  // ticketCode
    }
}