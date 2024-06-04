namespace ReservationSystem;

public class GuideMenuRL : View
{
    public static string Show(Guide guide)
    {
        WriteLine("\nGuide Menu:\n");
        ColourText.WriteColored("", "1 |", ConsoleColor.Cyan, $" View {guide.Name}'s tours");
        ColourText.WriteColored("\n", "2 |", ConsoleColor.Cyan, " Start upcoming tour");
        ColourText.WriteColored("\n", "3 |", ConsoleColor.Cyan, " Back to scanning zone\n");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}