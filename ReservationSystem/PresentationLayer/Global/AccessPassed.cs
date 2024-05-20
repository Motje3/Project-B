namespace ReservationSystem;

public class AccessPassed : View
{
    public static void WelcomeGuide(Guide g)
    {
        try { Console.Clear(); } catch { };
        Console.Write("\nWelcome ");
        ColourText.WriteColored("", g.Name, ConsoleColor.Cyan, "!\n");
        Console.WriteLine();
    }
    public static void WelcomeAdmin()
    {

        WriteLine($"\nWelcome Admin\n");
    }
}