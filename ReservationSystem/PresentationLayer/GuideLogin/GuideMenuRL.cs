namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:\n");
        WriteLine("1 | View personal tours");
        WriteLine("2 | Start The Upcoming Tour");
        WriteLine("3 | Back to Visitors Menu");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}