namespace ReservationSystem;

public class TicketInputRL : View
{
    public static string Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" ---------------------------- ");
        Console.Write("|  ");
        Console.ResetColor();
        Console.Write("Scan your unique ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("barcode");
        Console.Write("  |");
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(" ---------------------------- ");
        Console.ResetColor();
        return ReadLine();
    }
}