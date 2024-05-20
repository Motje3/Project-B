namespace ReservationSystem;

public class StartMenuRL : View
{
    public static string Show()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine(" ------------------");
        Console.WriteLine("|    Admin Menu    |");
        Console.WriteLine(" ------------------");
        Console.ResetColor();
        Console.WriteLine();

        Console.ResetColor();

        ColourText.WriteColored("1", " | ", ConsoleColor.Cyan);
        Console.WriteLine(" Assign a Different Guide to today's tours");

        ColourText.WriteColored("2", " | ", ConsoleColor.Cyan);
        Console.WriteLine(" Change the default guide's roaster (To be implemented)");

        ColourText.WriteColored("4", " | ", ConsoleColor.Cyan);
        WriteLine(" Exit");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}