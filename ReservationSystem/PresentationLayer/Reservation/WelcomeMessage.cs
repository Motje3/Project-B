using System.Runtime.CompilerServices;

namespace ReservationSystem;

public class WelcomeMessage : View
{
    public static void WelcomeYourTicketConfirmed()
    {
        Console.Clear();
        ColourText.WriteColoredLine("", "Welcome! ", ConsoleColor.Cyan, "Your Ticket is confirmed!\n");
    }
    public static void WelcomeToTheMuseum()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("********************************************");
        Console.WriteLine("*                                          *");
        ColourText.WriteColored("*          ", "Welcome to the Museum!", ConsoleColor.White);
        Console.ForegroundColor = ConsoleColor.Cyan; // Reset to cyan for the ending star
        Console.WriteLine("          *");
        Console.WriteLine("*                                          *");
        Console.WriteLine("*                                          *");
        Console.WriteLine("********************************************");
        Console.ResetColor();
        Console.WriteLine();
    }



}