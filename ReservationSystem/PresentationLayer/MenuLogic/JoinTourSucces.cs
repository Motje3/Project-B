namespace ReservationSystem;

public class JoinTourSuccesMessage : View
{
    public static void Show(Tour chosenTour)
    {
        ColourText.WriteColored("Tour at ", chosenTour.StartTime.ToString("HH:mm"), ConsoleColor.Cyan, " joined successfully!\n");

    }
    public static void Show2()
    {
        ColourText.WriteColoredLine("\n| Press ", "Enter", ConsoleColor.Cyan, " to return to the Ticket Scanner ");
        ColourText.WriteColoredLine("\n| Press ", "Space", ConsoleColor.Cyan, " to change your reservation details");

    }
    public static void Show3()
    {
        ColourText.WriteColoredLine("\nPress ", "Enter", ConsoleColor.Cyan, " to return to the Ticket Scanner");
    }
}