namespace ReservationSystem;

public class WelcomeMessage: View
{
    public static void Show()
    {
        WriteLine("\nWelcome! Your ticket is confirmed\n");
    }
    public static void WelcomeToTheMuseum()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("********************************************");
        Console.WriteLine("*                                          *");
        ColourText.WriteColoredLine("", "Welcome to the Museum!", ConsoleColor.Green, "");
        Console.WriteLine("                                          *");
        Console.WriteLine("*                                          *");
        Console.WriteLine("********************************************");
        Console.ResetColor();
        Console.WriteLine();
    }


}