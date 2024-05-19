namespace ReservationSystem;

public class TicketInputRL : View
{
    public static string Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;

        // Print the top border with padding
        Console.WriteLine(new string('-', 30));

        // Print the side padding with message
        Console.Write("|");
        Console.Write(new string(' ', 2)); // Left padding
        Console.ResetColor();
        ColourText.WriteColored("Scan your unique", " barcode", ConsoleColor.Cyan, "");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(new string(' ', 2)); // Right padding
        Console.WriteLine("|");

        // Print the bottom border with padding
        Console.WriteLine(new string('-', 30));

        // Reset the console color for further input
        Console.ResetColor();
        return ReadLine();
    }
}