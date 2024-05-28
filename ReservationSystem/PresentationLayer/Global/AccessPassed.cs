namespace ReservationSystem;

public class AccessPassed : View
{
    public static void WelcomeGuide(Guide g)
    {
        Write("\nWelcome ");
        ColourText.WriteColored("", g.Name, ConsoleColor.Cyan, "!\n");
        WriteLine("");
    }
    public static void WelcomeAdmin()
    {
        WriteLine($"\nWelcome Admin\n");
    }
}