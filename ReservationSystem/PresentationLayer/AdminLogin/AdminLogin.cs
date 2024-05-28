namespace ReservationSystem;


public class AdminLogin : View
{
    public static void Show()
    {
        try { Console.Clear(); } catch { }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine(" -------------------------");
        Console.WriteLine("|       Admin login       |");
        Console.WriteLine(" -------------------------");
        Console.ResetColor();
        Console.WriteLine("Password: "); // Ensure the cursor moves to the next line after the box

    }
}