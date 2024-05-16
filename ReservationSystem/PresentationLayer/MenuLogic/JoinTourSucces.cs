namespace ReservationSystem;

public class JoinTourSuccesMessage : View
{
    public static void Show(Tour chosenTour)
    {
        WriteLine($"Tour at {chosenTour.StartTime.ToString("h:mm tt")} joined successfully!");
    }
    public static void Show2()
    {
        WriteLine($"\nYou will be redirected to main page after 3 seconds\n");
        Thread.Sleep(2000);
        WriteLine($"3");
        Thread.Sleep(1000);
        WriteLine($"2");
        Thread.Sleep(1000);
        WriteLine($"1");
        Thread.Sleep(1000);
    }
}