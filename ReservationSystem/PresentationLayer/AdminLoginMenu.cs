using ReservationSystem;

public class AdminLoginMenu : View
{
    public static void ProcessLoginForm()
    {
        WriteLine("\nAdmin login\n");

        Write("Enter username: ");
        string username = ReadLine();
        Write("Enter password: ");
        string password = AdminBackEnd.ReadPassword();
        if (Authenticator.AuthenticateUser(username, password))
        {
            WriteLine("\nAccess Granted!\n");
            ShowAdminMenu();
        }
        else
        {
            WriteLine("\nAccess Denied. Invalid username or password.\n");
        }
    }

    public static void ShowAdminMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            WriteLine("\nAdmin Menu:");
            WriteLine("1. Change capacity of a tour");
            WriteLine("2. Change time of a tour");
            WriteLine("3. View Tours");
            WriteLine("4. Get advice for upcoming days");

            WriteLine("5. Exit");

            Write("\nEnter your choice: ");
            string choice = ReadLine();

            switch (choice)
            {
                case "1":
                    AdminBackEnd.ChangeTourCapacity();
                    break;
                case "2":
                    AdminBackEnd.ChangeTourTime();
                    break;
                case "3":
                    GuidedTour.PrintToursOpenToday();
                    break;
                case "4":
                    continueRunning = false;
                    break;
                default:
                    WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    

}
