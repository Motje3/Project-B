using ReservationSystem;
using System;

public class GuideLoginMenu : View
{
    public static void ProcessLoginForm(string userCode)
    {
        Guide.LoadGuides(); // Ensure guides are loaded

        GuideLogin.Show();
        string password = AdminBackEnd.ReadPassword();

        Guide authenticatedGuide = Guide.AuthenticateGuide(password);

        if (authenticatedGuide != null)
        {
            AccessPassed.WelcomeGuide(authenticatedGuide);
            ShowGuideMenu(authenticatedGuide);
        }
        else
        {
            AccessFailed.Show();
        }
    }

    public static void ShowGuideMenu(Guide guide)
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            string choice = GuideMenuRL.Show();

            switch (choice)
            {
                case "1":
                    Guide.ViewPersonalTours(guide);
                    break;
                case "2":
                guide.StartUpcomingTour();
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
