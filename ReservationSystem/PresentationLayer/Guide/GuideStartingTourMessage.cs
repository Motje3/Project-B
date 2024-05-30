using ReservationSystem;

public class GuideStartingTourMessage : View
{
    public static void Show(Tour upcomingTour)
    {
        Write($"Starting the Tour at ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{upcomingTour.StartTime.ToString("HH:mm")}");
        Console.ResetColor();
        Write(" with ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{upcomingTour.ExpectedVisitors.Count}");
        Console.ResetColor();
        Write(" expected visitors.");
        WriteLine("");


        Write($"\nBefore that, please scan the tickets for all the present visitors\nAnd once done, write '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"Start");
        Console.ResetColor();
        Write("' to continue to next menu, or '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("Q");
        Console.ResetColor();
        Write("' to go back to previous menu");
        WriteLine("");

        Write("\nCurrently the following tickets have been scanned: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"[ {string.Join(", ",upcomingTour.PresentVisitors.Select(v => v.TicketCode))} ]");
        Console.ResetColor();
        WriteLine("");
    }
}