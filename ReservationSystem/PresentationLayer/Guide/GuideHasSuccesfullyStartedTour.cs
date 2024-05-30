using ReservationSystem;

public class GuideHasSuccesfullyStartedTour : View
{
    public static void Show(Tour tour)
    {
        Write($"Tour at ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{tour.StartTime.ToString("HH:mm")}");
        Console.ResetColor();
        Write(" has been started successfully.\n");
    }
}