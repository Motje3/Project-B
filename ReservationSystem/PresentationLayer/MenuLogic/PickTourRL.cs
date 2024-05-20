namespace ReservationSystem;

public class PickTourRL : View
{
    public static string Show()
    {
        ColourText.WriteColored("\nPlease choose the ", "number ", ConsoleColor.Cyan, "left of the tour you want to join\n");
        return ReadLine(); // chosenTourNumber
    }
}