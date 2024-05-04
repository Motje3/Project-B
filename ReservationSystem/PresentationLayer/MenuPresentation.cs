public class MenuPresentation
{
    public static void ShowRestrictedMenu(Visitor visitor)
    {
        MenuLogic logic = new MenuLogic();
        bool loopOption = true;
        while (loopOption)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Join a tour");
            Console.WriteLine("2. Exit");
            Console.WriteLine("\nEnter your choice: ");
            string choice = Console.ReadLine();
            loopOption = logic.HandleRestrictedMenuChoice(choice, visitor);


        }
    }

    public static void ShowFullMenu(Visitor visitor)
    {
        MenuLogic logic = new MenuLogic();
        bool choosingOption = true;
        while (choosingOption)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Change a tour");
            Console.WriteLine("2. Cancel my tour reservation");
            Console.WriteLine("3. Exit");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();
            choosingOption = logic.HandleFullMenuChoice(choice, visitor);

            if (choice == "2" && !choosingOption) // Check if the tour cancellation was chosen and confirmed.
            {
                ShowRestrictedMenu(visitor); // Transition back to the restricted menu after cancellation.
                break; // Break the loop after transitioning to ensure we don't run this menu again immediately.
            }
        }
    }


    public static void PrintSuccessfullyJoinedTour(GuidedTour chosenTour)
    {
        Console.WriteLine("\nSuccessfully joined the following tour:");
        PrintTourString(chosenTour);
    }

    public static void PrintTourString(GuidedTour tour)
    {
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        string hour = tour.StartTime.Hour.ToString("00");
        string minute = tour.StartTime.Minute.ToString("00");
        Console.WriteLine($"{hour}:{minute} {tourDate} | duration: {tour.Duration} minutes\n");
    }
}
