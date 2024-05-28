namespace ReservationSystem;

public static class AdminMessages
{
    public static void ShowAddNewGuidedTourOptions()
    {
        Console.Clear();
        Console.WriteLine("Add New Guided Tour:");
        Console.WriteLine("1. Add a new guided tour for today");
        Console.WriteLine("2. Add a new guided tour to the standard schedule");
    }

    public static void ShowEnterTimePrompt()
    {
        Console.Write("Enter time for the tour (hh:mm): ");
    }

    public static void ShowInvalidTimeFormat()
    {
        Console.WriteLine("Invalid time format. Please enter the time in hh:mm format.\n");
    }

    public static void ShowNoGuidesAvailable()
    {
        Console.WriteLine("No guides available.\n");
    }

    public static void ShowChooseGuide(List<Guide> guides)
    {
        Console.WriteLine("Choose a guide:");
        for (int i = 0; i < guides.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {guides[i].Name}");
        }
    }

    public static void ShowInvalidGuideChoice()
    {
        Console.WriteLine("Invalid guide choice. Please select a valid guide number.\n");
    }

    public static void ShowTourAddedSuccessfully()
    {
        Console.WriteLine("Tour added successfully.\n");
    }

    public static void ShowInvalidOption()
    {
        Console.WriteLine("Invalid option. Please try again.");
    }

    public static void ShowWaitForUser()
    {
        ColourText.WriteColoredLine("| Press ", "Space", ConsoleColor.Cyan, " to go back to the Menu");
    }

    public static void ShowReturningToMenu()
    {
        Console.WriteLine("Returning to the menu...");
    }
}