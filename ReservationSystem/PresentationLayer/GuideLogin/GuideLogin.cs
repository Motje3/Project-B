namespace ReservationSystem;

public class GuideLogin : View
{
    public static void Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine();
        Console.WriteLine(" ------------------");
        Console.WriteLine("|    Guide Login   |");
        Console.WriteLine(" ------------------");
        Console.ResetColor();
        Console.WriteLine();
        Write("Enter password: ");
    }
}