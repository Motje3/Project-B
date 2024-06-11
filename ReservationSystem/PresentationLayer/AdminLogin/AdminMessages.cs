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
        ColourText.WriteColoredLine("Enter time for the new tour", " (HH:MM format):", ConsoleColor.Cyan);
    }

    public static void ShowInvalidTimeFormat()
    {
        ColourText.WriteColoredLine("Invalid time format.", ConsoleColor.Red, " Please enter the time in HH:MM format.\n,", "");
    }

    public static void ShowNoGuidesAvailable()
    {
        WriteLine("No guides available.\n");
    }

    public static void ShowChooseGuide(List<Guide> guides)
    {
        WriteLine("\nChoose a guide: \n");
        for (int i = 0; i < guides.Count; i++)
        {
            ColourText.WriteColoredLine("", $"{i + 1} | ", ConsoleColor.Cyan, $"{guides[i].Name} \n");
        }
    }

    public static void ShowInvalidGuideChoice()
    {
        ColourText.WriteColoredLine("Invalid guide choice", ConsoleColor.Red, " Please try again.\n");
    }

    public static void ShowTourAddedSuccessfully(Guide guideEntry, DateTime tourStartTime)
    {
        string formattedtime = tourStartTime.ToString("HH:mm");
        ColourText.WriteColoredLine("Tour added ", "successfully", ConsoleColor.Green, $" at {formattedtime} and assigned to {guideEntry.Name}.\n");
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