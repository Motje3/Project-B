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

        ColourText.WriteColored("", "1 | ", ConsoleColor.Cyan);
        Console.WriteLine(" Assign a Different Guide to today's tours");

        ColourText.WriteColored("", "2 | ", ConsoleColor.Cyan);
        Console.WriteLine(" Edit the guided tours schedule");

        ColourText.WriteColored("", "3 | ", ConsoleColor.Cyan);
        WriteLine(" Return to Scanning Zone");

        ColourText.WriteColored("\nEnter your ", "choice: ", ConsoleColor.Cyan);

        return ReadLine();
    }
}