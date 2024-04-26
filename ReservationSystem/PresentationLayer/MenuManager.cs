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


        public static void ShowFullMenu(Visitor visitorcode)
        {
            bool choosingOption = true;
            while (choosingOption)
            {


                Console.WriteLine("Please choose an option:");
                Console.WriteLine("1. Join a different tour");
                Console.WriteLine("2. Cancel my tour reservation");
                Console.WriteLine("3. Exit");

                Console.Write("\nEnter your choice: \n");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        bool selectingTour = true;
                        while (selectingTour)
                        {
                            Console.WriteLine("\nYour current tour reservation is:");
                            _printTourString(GuidedTour.FindTourById(visitorcode.AssingedTourId));

                            List<GuidedTour> allowedTours = GuidedTour.PrintToursOpenToday();

                            Console.WriteLine($"\nPlease choose a number next to the tour you wish to join");

                            // Visitor has to choose a tour
                            string chosenTourNumber = Console.ReadLine();
                            int.TryParse(chosenTourNumber, out int tourNumber);
                            while (tourNumber <= 0 || tourNumber > allowedTours.Count)
                            {
                                Console.WriteLine("Invalid choice, please choose a number next to the tour you wish to join");
                                chosenTourNumber = Console.ReadLine();
                                int.TryParse(chosenTourNumber, out tourNumber);
                            }
                            GuidedTour chosenTour = allowedTours[tourNumber - 1];
                            GuidedTour visitorsTour1 = GuidedTour.FindTourById(visitorcode.AssingedTourId);
                            Visitor visitor = Visitor.FindVisitorByTicketCode(visitorcode.TicketCode);

                            // Perform the transfer
                            if (visitorsTour1 != null)
                            {
                                visitorsTour1.TransferVisitor(visitor, chosenTour);
                                Console.WriteLine($"\nYou have successfully transferred to the new tour: {chosenTour.StartTime}.\n");
                            }
                            else
                            {
                                Console.WriteLine("\nYou are not currently registered on any tour.");
                            }
                            visitorcode =visitor;
                            selectingTour = false;
                        }
                        break;


                    case "2":
                        GuidedTour visitorsTour = GuidedTour.FindTourById(visitorcode.AssingedTourId);

                        if (visitorsTour == null)
                        {
                            Console.WriteLine("Error: Unable to find the tour.");
                            break;
                        }

                        // Remove the visitor from the tour
                        visitorsTour.RemoveVisitor(visitorcode);

                        Console.WriteLine("Tour reservation canceled successfully.");

                        MenuManager.ShowRestrictedMenu(visitorcode.TicketCode);
                        return;

                    case "3":
                        choosingOption = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }

        private static void _printSuccesfullyJoinedTour(GuidedTour chosenTour)
        {
            Console.WriteLine("\nSuccesfully joined the following tour:");
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
