using ReservationSystem;

public class GuideLoginMenu : View
{
    public static void ProcessLoginForm(string userCode)
    {
        GuideLogin.Show();
        string password = AdminBackEnd.ReadPassword();


        if (GuideMenuBackEnd.AuthenticateGuide(password))
        {
            AccessPassed.Show();
            ShowGuideMenu();
        }
        else
        {
            AccessFailed.Show();
        }
    }

    public static void ShowGuideMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            // Console.WriteLine("\nGuide Menu:");
            // Console.WriteLine("1. View personal tours(To Be Implemented)");
            // Console.WriteLine("2. Exit");

            // Write("\nEnter your choice: ");
            string choice = GuideMenuRL.Show();

            switch (choice)
            {
                case "1":
                    //To be implemented
                    break;
                case "2":
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