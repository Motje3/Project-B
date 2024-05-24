using ReservationSystem;

public class AdminLoginMenu : View
{
    public static void ProcessLoginForm()
    {

        AdminLogin.Show();
        string password = AdminBackEnd.ReadPassword().ToLower();

        if (Authenticator.AuthenticateUser(password))
        {
            AccessPassed.WelcomeAdmin();
            ShowAdminMenu();
        }
        else
        {
            Console.WriteLine(password);
            AccessFailed.Show();
        }
    }

    public static void ShowAdminMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {

            string choice = StartMenuRL.Show();

            switch (choice)
            {
                case "1":
                    Guide.ReassignGuideToTour();
                    break;
                case "2":
                    AdminBackEnd.AddNewGuidedTour();
                    break;
                case "3":
                    continueRunning = false;
                    try { Console.Clear(); } catch { }
                    break;
                default:
                    InvalidRL.Show();
                    break;
            }
        }
    }



}
