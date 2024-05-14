namespace ReservationSystem;

public class PickTour : View
{
    public static string Show()
    {
        WriteLine("\nPlease choose a number next to the tour you wish to join:");
        return ReadLine(); // chosenTourNumber
    }
}