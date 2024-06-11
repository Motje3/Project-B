namespace ReservationSystem;

public class GuideMenuRL : View
{
        public static string Show(Guide guide)
        {
                WriteLine("\nGuide Menu:\n");
                ColourText.WriteColored("", "1 |", ConsoleColor.Cyan, $" View my tours for today");
                ColourText.WriteColored("\n", "2 |", ConsoleColor.Cyan, " Start my upcoming Tour");
                ColourText.WriteColored("\n", "3 |", ConsoleColor.Cyan, " Back to Ticket Scanner\n");

                Write("\nEnter your choice: ");
                return ReadLine();
        }
}