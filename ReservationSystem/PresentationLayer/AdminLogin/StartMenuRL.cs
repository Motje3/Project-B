namespace ReservationSystem;

public class StartMenuRL : View
{
    public static string Show()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine("");
        Program.World.WriteLine(" ------------------");
        Program.World.WriteLine("|    Admin Menu    |");
        Program.World.WriteLine(" ------------------");
        Console.ResetColor();
        Program.World.WriteLine("");

        Console.ResetColor();

        ColourText.WriteColored("", "1 | ", ConsoleColor.Cyan);
        Program.World.WriteLine(" Assign a Different Guide to today's tours");

        ColourText.WriteColored("", "2 | ", ConsoleColor.Cyan);
        Program.World.WriteLine(" Edit the guided tours schedule");

        ColourText.WriteColored("", "3 | ", ConsoleColor.Cyan);
        WriteLine(" Return to Scanning Zone");

        ColourText.WriteColored("\nEnter your ", "choice: ", ConsoleColor.Cyan);

        return ReadLine();
    }
}