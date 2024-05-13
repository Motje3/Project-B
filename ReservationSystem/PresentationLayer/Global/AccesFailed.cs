namespace ReservationSystem;

public class AccessFailed : View
{
    public static void Show()
    {
        WriteLine("\nAccess Denied. Invalid username or password.\n");
    }
}