// this can be re-used for multiple classes
namespace ReservationSystem;

public class InvalidRL : View
{
    public static void Show(string subMessage = "Please enter a valid option.")
    {
        try { Console.Clear(); } catch { }
        Console.ForegroundColor = ConsoleColor.Red;
        Program.World.WriteLine("************************");
        Program.World.WriteLine("*    Invalid choice    *");
        Program.World.WriteLine("************************");
        Program.World.WriteLine("");
        Console.ResetColor();

        Program.World.Write($"{subMessage}\n");

    }
}