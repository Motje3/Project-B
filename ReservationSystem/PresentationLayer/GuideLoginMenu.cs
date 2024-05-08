using ReservationSystem;

public class GuideLoginMenu : View
{
    public static void ProcessLoginForm(string userCode)
    {
        WriteLine("\nGuide login\n");
        Write("Enter guide code: ");
        string guideCode = ReadLine();
        Write("Enter password: ");
        string password = AdminBackEnd.ReadPassword();

        GuideBackEnd.LoadMyGuide(guideCode);

        if (GuideBackEnd.AuthenticateGuide(password))
        {
            WriteLine("\nAccess Granted!\n");
            ShowGuideMenu();
        }
        else
        {
            WriteLine("\nAccess Denied. Invalid password.\n");
        }
    }

    public static void ShowGuideMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            WriteLine("\nGuide Menu:");
            WriteLine("1. View personal tours");
            WriteLine("2. Start a tour and check attendings");
            
            WriteLine("3. Exit");

            Write("\nEnter your choice: ");
            string choice = ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayGuideTours();
                    break;
                case "2":
                    continueRunning = false;
                    break;
                default:
                    WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void DisplayGuideTours()
    {
        var tours = GuideBackEnd.GetGuideTours();
        if (tours.Any())
        {
            foreach (var tour in tours)
            {
                WriteLine($"Tour on {tour.StartTime.ToShortDateString()} from {tour.StartTime.ToShortTimeString()} to {tour.EndTime.ToShortTimeString()} with {tour.ExpectedVisitors.Count} visitors expected.");
            }
        }
        else
        {
            WriteLine("No tours currently assigned.");
        }
    }


}