using System.Globalization;

public static class MenuManager
{
    public static void ShowRestrictedMenu(Visitor visitor)
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
                    // Print all avaible tours could be simplified in a methode that is in guidedtour. 
                    List<GuidedTour> allowedTours = new();
                    int allowedTourIndex = 0;
                    List<GuidedTour> todayTours = GuidedTour.ReturnAllCurrentToursFromToday();
                    for (int tourIndex = 0; tourIndex < todayTours.Count; tourIndex++)
                    {
                        GuidedTour currentTour = todayTours[tourIndex];
                        int spacesLeftInTour = currentTour.MaxCapacity - currentTour.ExpectedVisitors.Count;
                        if (spacesLeftInTour >= 0) 
                        {
                            allowedTours.Add(currentTour);
                            Console.WriteLine($"{allowedTourIndex+1} | {TimeOnly.FromDateTime(currentTour.StartTime)} | {currentTour.Duration} minutes | {spacesLeftInTour} places remaining ");
                            allowedTourIndex++;
                        }
                    }
                    Console.WriteLine($"\nPlease choose a number next to the tour you wish to join");

                    // Visitor has to choose a tour
                    string chosenTourNumber = Console.ReadLine();
                    int tourNumber;
                    int.TryParse(chosenTourNumber, out tourNumber);
                    bool outSideOfAllowedRange = tourNumber < 0 && tourNumber > allowedTours.Count;
                    while (outSideOfAllowedRange)
                    {
                        chosenTourNumber = Console.ReadLine();
                        int.TryParse(chosenTourNumber, out tourNumber);
                    }
                    GuidedTour chosenTour = allowedTours[tourNumber - 1];

                    // Add visitor from to chosenTour
                    //chosenTour.AddVisitor(visitor);
                    break;

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
                        var ticket = Ticket.Tickets.FirstOrDefault(t => t.TicketCode == visitor.TicketCode);
                        if (ticket == null)
                        {
                            Console.WriteLine("Ticket code is not valid.");
                            break; // Exit the switch case and the while loop
                        }

                        // Print all avaible tours
                        List<GuidedTour> allowedTours = new();
                        int allowedTourIndex = 0;
                        for (int tourIndex = 0; tourIndex < GuidedTour.ReturnAllCurrentToursFromToday().Count; tourIndex++)
                        {
                            GuidedTour currentTour = GuidedTour.CurrentTours[tourIndex];
                            int spacesLeftInTour = currentTour.MaxCapacity - currentTour.ExpectedVisitors.Count;
                            if (spacesLeftInTour <= 0) 
                            {
                                allowedTours.Add(currentTour);
                                Console.WriteLine($"{allowedTourIndex+1} | {TimeOnly.FromDateTime(currentTour.StartTime)} | {currentTour.Duration} minutes | {spacesLeftInTour} places remaining ");
                                allowedTourIndex++;
                            }
                        }
                        Console.WriteLine($"\nPlease choose a number next to the tour you wish to join");

                        string chosenTourNumber = Console.ReadLine();
                        int tourNumber;
                        int.TryParse(chosenTourNumber, out tourNumber);
                        bool outSideOfAllowedRange = tourNumber < 0 && tourNumber > allowedTours.Count;
                        while (outSideOfAllowedRange)
                        {
                            chosenTourNumber = Console.ReadLine();
                            int.TryParse(chosenTourNumber, out tourNumber);
                        }
                        GuidedTour chosenTour = allowedTours[tourNumber - 1];

                        // Transfer visitor from currentTour to chosenTour

                        // visitor.ReservedTour.TransferVisitor(visitor, chosenTour);
                    }
                    break;
                case "2":
                    // visitor.ReservedTour.RemoveVisitor(visitor);
                    choosingOption = false; // Exit the main menu loop after cancelling.
                    break;
                case "3":
                    choosingOption = false; // Exit the main menu loop.
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }


}
