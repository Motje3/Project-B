namespace ReservationSystem;

public class GidsAccessFailed : View
{
    public void Show()
    {
        WriteLine("\nAccess Denied, invalid password. Returning to start menu in 3 seconds\n");
    }
}