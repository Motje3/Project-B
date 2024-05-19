namespace ReservationSystem;

public class JoinTourSuccesMessage : View
{
    public static void Show(Tour chosenTour)
    {
        WriteLine($"Tour at {chosenTour.StartTime.ToString("h:mm tt")} joined successfully!");
    }
    public static void Show2()
    {
        ColourText.WriteColoredLine("\n| Press ", "Enter", ConsoleColor.Green, " to return to the Main Menu ");
        ColourText.WriteColoredLine("| Press", "Space", ConsoleColor.Green, " to change your reservation details");

    }
    public static void Show3()
    {
        ColourText.WriteColoredLine("\nPress ", "Enter", ConsoleColor.Green, " to return to the Main Menu");
    }
}