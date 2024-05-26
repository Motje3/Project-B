namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:\n");
        WriteLine("1 | View personal tours");
        WriteLine("2 | Start Upcoming Tour");
        WriteLine("3 | Add visitor for upcoming tour"); ;
        WriteLine("4 | Back to visitor's Menu");

        Write("\nEnter your choice: ");
        return ReadLine(); // choice.
    }
}