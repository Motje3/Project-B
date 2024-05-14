namespace ReservationSystem;

public class ChangeTourRL : View
{
    public static string Show()
    {
        WriteLine("\nPlease choose a number next to the new tour you wish to join");
        return ReadLine();
    }
}