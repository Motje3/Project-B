namespace ReservationSystem;

public class InvalidGuide : View
{
    public static void Show()
    {
        ColourText.WriteColoredLine("\n", "Invalid ", ConsoleColor.DarkRed, "guide selected.\n");
    }
}