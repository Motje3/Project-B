using ReservationSystem;

public class GuideAddingVisitorLastMinute : View
{
    public static void Show()
    {
        Write("Scan the ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("ticket");
        Console.ResetColor();
        Write(" of the visitor you want to add or type '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("Start");
        Console.ResetColor();
        Write("' to start the tour\n");
    }
}