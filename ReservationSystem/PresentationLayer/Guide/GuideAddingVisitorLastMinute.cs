using ReservationSystem;

public class GuideAddingVisitorLastMinute : View
{
    public static void Show()
    {
        
        Write("If there are any additional visitors please scan their ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("ticket");
        Console.ResetColor();
        Write(" to add them to the tour or type '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("Start");
        Console.ResetColor();
        Write("' to start the tour\n");
    }
}