namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:\n");
        WriteLine("1 | View personal tours");
        WriteLine("2 | Exit");
        WriteLine("3 | Start The Upcoming Tour");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}