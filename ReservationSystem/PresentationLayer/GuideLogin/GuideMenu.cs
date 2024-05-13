namespace ReservationSystem;

public class GuideMenu : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:");
        WriteLine("1. View personal tours(To Be Implemented)");
        WriteLine("2. Exit");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}