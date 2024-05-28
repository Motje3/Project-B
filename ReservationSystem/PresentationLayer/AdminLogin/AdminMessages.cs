namespace ReservationSystem;

public class AdminMessages : View
{
    public static void ShowAddNewGuidedTourOptions()
    {
        Console.Clear();
        WriteLine("Add New Guided Tour:");
        WriteLine("1. Add a new guided tour for today");
        WriteLine("2. Add a new guided tour to the standard schedule");
    }

    public static void ShowEnterTimePrompt()
    {
        Write("Enter time for the tour (hh:mm): ");
    }

    public static void ShowInvalidTimeFormat()
    {
        WriteLine("Invalid time format. Please enter the time in hh:mm format.\n");
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
            WriteLine($"{i + 1}. {guides[i].Name}");
        }
    }

    public static void ShowInvalidGuideChoice()
    {
        WriteLine("Invalid guide choice. Please select a valid guide number.\n");
    }

    public static void ShowTourAddedSuccessfully()
    {
        WriteLine("Tour added successfully.\n");
    }

    public static void ShowInvalidOption()
    {
        WriteLine("Invalid option. Please try again.");
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