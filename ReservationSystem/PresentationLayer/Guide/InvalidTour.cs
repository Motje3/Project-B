namespace ReservationSystem;

public class InvalidTour : View
{
    public static void Show()
    {
        ColourText.WriteColoredLine("\n", "Invalid ", ConsoleColor.DarkRed, "tour selected.\n");
    }
}