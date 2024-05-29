namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nGuide Menu:\n");
        ColourText.WriteColored("", "1 |", ConsoleColor.Cyan, " View personal Tours");
        ColourText.WriteColored("", "2 |", ConsoleColor.Cyan, " Start Upcoming Tour");
        ColourText.WriteColored("", "3 |", ConsoleColor.Cyan, " Add visitor for upcoming tour");
        ColourText.WriteColored("", "4 |", ConsoleColor.Cyan, " Back to visitor's Menu");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}