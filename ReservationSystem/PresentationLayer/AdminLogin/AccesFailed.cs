namespace ReservationSystem;

public class AccessFailed : View
{
    public void Show()
    {
        WriteLine("\nAccess Denied. Invalid username or password.\n");
    }
}