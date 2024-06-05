namespace ReservationSystem;


public class AdminLogin : View
{
    public static void Show()
    {
        try { Console.Clear(); } catch { }

        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine("");
        Program.World.WriteLine(" -------------------------");
        Program.World.WriteLine("|       Admin login       |");
        Program.World.WriteLine(" -------------------------");
        Console.ResetColor();
        Program.World.WriteLine("Password: "); // Ensure the cursor moves to the next line after the box

    }
}