using System.Globalization;

public static class MenuManager
{
    public static void ShowRestrictedMenu(string visitorCode)
    {
        bool loopOption = true;
        while (loopOption)
        {
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Join a tour");
            Console.WriteLine("2. Exit");
            Console.WriteLine("\nEnter your choice: ");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    // Print all avaible tours
                    List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();

                    Console.WriteLine($"\nPlease choose a number next to the tour you wish to join");

                    // Visitor has to choose a tour
                    string chosenTourNumber = Console.ReadLine();
                    int tourNumber;
                    int.TryParse(chosenTourNumber, out tourNumber);
                    bool outSideOfAllowedRange = tourNumber <= 0 || tourNumber > allowedTours.Count;
                    while (outSideOfAllowedRange)
                    {
                        Console.WriteLine("Invalid choice, please choose a number next to the tour you wish to join");
                        chosenTourNumber = Console.ReadLine();
                        int.TryParse(chosenTourNumber, out tourNumber);
                        outSideOfAllowedRange = tourNumber <= 0 || tourNumber > allowedTours.Count;
                    }
                    GuidedTour chosenTour = allowedTours[tourNumber - 1];

                    // Add visitor from to chosenTour
                    Visitor toBeAdded = new(visitorCode);
                    chosenTour.AddVisitor(toBeAdded);
                    _printSuccesfullyJoinedTour(chosenTour);

                    ShowFullMenu(toBeAdded);
                    return;

                case "2":
                    loopOption = false;
                    break;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }


    public static void ShowFullMenu(Visitor visitor)
    {
        bool choosingOption = true;
        while (choosingOption)
        {
            Console.WriteLine("Your current tour reservation is:");
            //_printTourString(GuidedTour.FindTourById(visitor.ReservedTourId));
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Join a different tour");
            Console.WriteLine("2. Cancel my tour reservation");
            Console.WriteLine("3. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    bool selectingTour = true;
                    while (selectingTour)
                    {
                        // Print all avaible tours
                        List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();

                        Console.WriteLine($"\nPlease choose a number next to the tour you wish to join");

                        // Visitor has to choose a tour
                        string chosenTourNumber = Console.ReadLine();
                        int tourNumber;
                        int.TryParse(chosenTourNumber, out tourNumber);
                        bool outSideOfAllowedRange = tourNumber <= 0 || tourNumber > allowedTours.Count;
                        while (outSideOfAllowedRange)
                        {
                            Console.WriteLine("Invalid choice, please choose a number next to the tour you wish to join");
                            chosenTourNumber = Console.ReadLine();
                            int.TryParse(chosenTourNumber, out tourNumber);
                            outSideOfAllowedRange = tourNumber <= 0 || tourNumber > allowedTours.Count;
                        }
                        GuidedTour chosenTour = allowedTours[tourNumber - 1];

                        // Transfer visitor from currentTour to chosenTour
                        //visitorsTour = GuidedTour.FindTourById(visitor.ReservedTourId);
                        //visitorsTour.TransferVisitor(visitor, chosenTour);

                        _printSuccesfullyJoinedTour(chosenTour);
                        selectingTour = false;
                    }
                    break;

                case "2":
                    // Find the tour the visitor is reserved for using the AssingedTourId
                    GuidedTour visitorsTour = GuidedTour.FindTourById(visitor.AssingedTourId.Value);

                    if (visitorsTour == null)
                    {
                        Console.WriteLine("Error: Unable to find the tour.");
                        break;
                    }

                    // Remove the visitor from the tour
                    visitorsTour.RemoveVisitor(visitor);

                    Console.WriteLine("Tour reservation canceled successfully.");

                    // Show the restricted menu to the visitor after canceling the reservation
                    MenuManager.ShowRestrictedMenu(visitor.TicketCode);
                    return;

                case "3":
                    choosingOption = false; // Exit the main menu loop.
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void _printSuccesfullyJoinedTour(GuidedTour chosenTour)
    {
        Console.WriteLine("Succesfully joined the following tour:");
        _printTourString(chosenTour);
    }

    private static void _printTourString(GuidedTour tour)
    {
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        string hour = tour.StartTime.Hour.ToString();
        string minute = tour.StartTime.Minute.ToString();
        if (minute == "0")
            minute = "00";

        Console.WriteLine($"{hour}:{minute} {tourDate} | duration: {tour.Duration} minutes\n");
    }

}
