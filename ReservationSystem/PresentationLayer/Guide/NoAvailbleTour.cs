namespace ReservationSystem;

public class NoAvailbleTour : View
{
    public static void Show()
    {
        ColourText.WriteColoredLine("\n", "No availble tours for today to add a visitor ", ConsoleColor.DarkRed, "\n");
    }
}