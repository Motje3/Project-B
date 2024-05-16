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

            string choice = GuideMenuRL.Show();

            switch (choice)
            {
                case "1":
                    //option to show this guides own guided tours for today. 
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