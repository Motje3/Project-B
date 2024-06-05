namespace ReservationSystem;

public class TicketInputRL : View
{
    public static string Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine(" ---------------------------- ");
        Console.Write("|  ");
        Console.ResetColor();
        Console.Write("Scan your unique ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("barcode");
        Console.Write("  |");
        Program.World.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine(" ---------------------------- ");
        Console.ResetColor();
        return ReadLine();
    }
}