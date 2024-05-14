namespace ReservationSystem;

public class ChangeTourSucces : View
{
    public static void Show(Tour chosenTour)
    {
        WriteLine($"\nYou have successfully transferred to the new tour: {chosenTour.StartTime.ToString("h:mm tt")}.\n");
    }
}