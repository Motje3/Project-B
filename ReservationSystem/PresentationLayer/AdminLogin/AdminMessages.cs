namespace ReservationSystem;

public class AdminMessages : View
{
    public static void ShowEditStandardSchedule()
    {
        Console.Clear();
        WriteLine("Add a new guided tour to the standard schedule\n");
    }

    public static void ShowEnterTimePrompt()
    {
        ColourText.WriteColoredLine("Enter time for the new tour", " (hh:mm):", ConsoleColor.Cyan);
    }

    public static void ShowInvalidTimeFormat()
    {
        ColourText.WriteColoredLine("Invalid time format.", ConsoleColor.Red, " Please enter the time in hh:mm format.\n");
    }

    public static void ShowNoGuidesAvailable()
    {
        WriteLine("No guides available.\n");
    }

    public static void ShowChooseGuide(List<Guide> guides)
    {
        WriteLine("Choose a guide:");
        for (int i = 0; i < guides.Count; i++)
        {
            WriteLine($"{i + 1}. {guides[i].Name}\n");
        }
    }

    public static void ShowInvalidGuideChoice()
    {
        ColourText.WriteColoredLine("Invalid guide choice", ConsoleColor.Red, " Please try again.\n");
    }

    public static void ShowTourAddedSuccessfully()
    {
        WriteLine("Tour added successfully.\n");
    }

    public static void ShowInvalidOption()
    {
        ColourText.WriteColoredLine("Invalid choice", ConsoleColor.Red, " Please try again.\n");
    }

    public static void ShowWaitForUser()
    {
        ColourText.WriteColoredLine("| Press ", "Space", ConsoleColor.Cyan, " to go back to the Menu");
    }

    public static void ShowReturningToMenu()
    {
        WriteLine("Returning to the menu...");
    }
}