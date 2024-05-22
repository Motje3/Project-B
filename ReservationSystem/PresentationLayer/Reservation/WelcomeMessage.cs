using System.Runtime.CompilerServices;

namespace ReservationSystem;

public class WelcomeMessage : View
{
    public static void WelcomeYourTicketConfirmed()
    {
        try { Console.Clear(); } catch { }
        ColourText.WriteColoredLine("", "Welcome! ", ConsoleColor.Cyan, "Your Ticket is confirmed!\n");
    }
    public static void WelcomeToTheMuseum()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine("********************************************");
        Program.World.WriteLine("*                                          *");
        ColourText.WriteColored("*          ", "Welcome to the Museum!", ConsoleColor.White);
        Console.ForegroundColor = ConsoleColor.Cyan; // Reset to cyan for the ending star
        Program.World.WriteLine("          *");
        Program.World.WriteLine("*                                          *");
        Program.World.WriteLine("*                                          *");
        Program.World.WriteLine("********************************************");
        Console.ResetColor();
        Program.World.WriteLine("");
    }



}