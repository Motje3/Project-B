namespace ReservationSystem;

public class ChangeTourSucces : View
{
    public static void Show(Tour chosenTour)
    {
        ColourText.WriteColoredLine("\nYou have successfully transferred to the new tour at ", chosenTour.StartTime.ToString("HH:mm"), ConsoleColor.Cyan, "\n");
    }
}