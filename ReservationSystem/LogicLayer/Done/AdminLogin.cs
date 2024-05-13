namespace ReservationSystem;

public class AdminLoginMenu : View
{
    public static void ProcessLoginForm()
    {
        AdminLogin AL = new AdminLogin();
        AccessPassed AP = new AccessPassed();
        AccessFailed AF = new AccessFailed();

        AL.Show();
        string password = AdminBackEnd.ReadPassword();

        if (Authenticator.AuthenticateUser(password))
        {
            AP.Show();
            ShowAdminMenu();
        }
        else
        {
            AF.Show();
        }
    }

    public static void ShowAdminMenu()
    {
        StartMenu SM = new StartMenu();
        InvalidRL I_RL = new InvalidRL();

        bool continueRunning = true;
        while (continueRunning)
        {
            // Console.WriteLine("\nAdmin Menu:");
            // Console.WriteLine("1. Assign a Different Guide to today's tours");
            // Console.WriteLine("2. Change the default guide's roaster (To be implemented)");
            // Console.WriteLine("4. Exit");

            // WriteLine("5. Exit");

            // Write("\nEnter your choice: ");
            string choice = SM.Show;

            switch (choice)
            {
                case "1":
                    Guide.ReassignGuideToTour();
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
                    I_RL.Show()
                    break;
            }
        }
    }

    

}
