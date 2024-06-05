namespace ReservationSystem;

public class GuideLogin : View
{
    public static void Show()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Program.World.WriteLine("");
        Program.World.WriteLine(" ------------------");
        Program.World.WriteLine("|    Guide Login   |");
        Program.World.WriteLine(" ------------------");
        Console.ResetColor();
        Program.World.WriteLine("");
        Write("Enter password: ");
    }
}