namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:\n");
        WriteLine("1 | View personal tours");
        WriteLine("2 | Add visitor for next tour");
        WriteLine("3 | Exit");

        Write("\nEnter your choice: ");
        return ReadLine(); // choice.
    }
}