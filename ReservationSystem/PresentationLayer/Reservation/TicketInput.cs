namespace ReservationSystem;

public class TicketInputRL : View
{
    public static string Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine(" ---------------------------- ");
        Program.World.Write("|  ");
        Console.ResetColor();
        Program.World.Write("Scan your unique ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.Write("barcode");
        Program.World.Write("  |");
        Program.World.WriteLine("");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine(" ---------------------------- ");
        Console.ResetColor();
        return ReadLine();
    }
}