public class MenuLogic
{
    public bool HandleRestrictedMenuChoice(string choice, string visitorCode)
    {
        switch (choice)
        {
            case "1":
                var allowedTours = GuidedTour.PrintToursOpenToday();
                return JoinTour(visitorCode, allowedTours);
            case "2":
                return false; // Exit the loop
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }

    public bool HandleFullMenuChoice(string choice, Visitor visitor)
    {
        switch (choice)
        {
            case "1":
                return ChangeTour(visitor);
            case "2":
                CancelTour(visitor);
                return true;
            case "3":
                return false; // Exit the loop
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                return true;
        }
    }

    private bool JoinTour(string visitorCode, List<GuidedTour> allowedTours)
    {
        Console.WriteLine("\nPlease choose a number next to the tour you wish to join");
        int tourNumber = int.Parse(Console.ReadLine());
        GuidedTour chosenTour = allowedTours[tourNumber - 1];
        Visitor toBeAdded = new Visitor(visitorCode);
        chosenTour.AddVisitor(toBeAdded);
        MenuPresentation.PrintSuccessfullyJoinedTour(chosenTour);
        return false; // Exit after joining
    }

    private bool ChangeTour(Visitor visitor)
    {
        // Logic for changing tours
        return false; // Return false if the menu should close
    }

    private void CancelTour(Visitor visitor)
    {
        // Logic for cancelling tours
    }
}
