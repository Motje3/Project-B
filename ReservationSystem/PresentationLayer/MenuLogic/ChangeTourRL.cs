namespace ReservationSystem;

public class ChangeTourRL : View
{
    public static string Show()
    {
        Program.World.Write("\nPlease choose the ");
        ColourText.WriteColored("", "number", ConsoleColor.Cyan, " left of the new tour you wish to join\n");
        Program.World.Write("Or type '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.Write("Q");
        Console.ResetColor();
        Program.World.Write("' to go back\n");
        return ReadLine();
    }
}