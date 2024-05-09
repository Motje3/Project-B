public static class AdminLoginMenu
{
    public static void ProcessLoginForm()
    {
        Console.WriteLine("\nAdmin login\n");

        
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (Authenticator.AuthenticateUser( password))
        {
            Console.WriteLine("\nAccess Granted!\n");
            ShowAdminMenu();
        }
        else
        {
            Console.WriteLine("\nAccess Denied. Invalid username or password.\n");
        }
    }

    public static void ShowAdminMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. Assign a guide to today's tours");
            Console.WriteLine("2. Assign guides for all upcoming tours.");
            Console.WriteLine("2. Change time of a tour");
            Console.WriteLine("3. View Tours");
            Console.WriteLine("4. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Guide.AssignGuideToTour();
                    break;
                case "2":
                    AdminBackEnd.ChangeTourTime();
                    break;
                case "3":
                    //Tour.PrintToursOpenToday();
                    break;
                case "4":
                    continueRunning = false;
                    try { Console.Clear(); } catch { }
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

}
