namespace ReservationSystem;

public class AccessFailed : View
{
    public static void Show()
    {
        ColourText.WriteColoredLine("\n", "Access Denied. ", ConsoleColor.DarkRed, "Invalid username or password.\n");
    }
}