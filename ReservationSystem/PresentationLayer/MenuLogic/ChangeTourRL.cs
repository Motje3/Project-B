namespace ReservationSystem;

public class ChangeTourRL : View
{
    public static string Show()
    {
        Console.Write("\nPlease choose the ");
        ColourText.WriteColored("", "number", ConsoleColor.Cyan, " left of the new tour you wish to join\n");

        return ReadLine();
    }
}