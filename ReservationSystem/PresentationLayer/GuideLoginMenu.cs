using Newtonsoft.Json;

public static class GuideLoginMenu
{
    public static void ProcessLoginForm(string userCode)
    {
        Console.WriteLine("\nGuide login\n");
        Console.Write("Enter guide code: ");
        string guideCode = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        GuideBackEnd.LoadMyGuide(guideCode);

        if (GuideBackEnd.AuthenticateGuide(password))
        {
            Console.WriteLine("\nAccess Granted!\n");
            ShowGuideMenu();
        }
        else
        {
            Console.WriteLine("\nAccess Denied. Invalid password.\n");
        }
    }

    public static void ShowGuideMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            Console.WriteLine("\nGuide Menu:");
            Console.WriteLine("1. View personal tours");
            Console.WriteLine("2. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    DisplayGuideTours();
                    break;
                case "2":
                    continueRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
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
                Console.WriteLine($"Tour on {tour.StartTime.ToShortDateString()} from {tour.StartTime.ToShortTimeString()} to {tour.EndTime.ToShortTimeString()} with {tour.ExpectedVisitors.Count} visitors expected.");
            }
        }
        else
        {
            Console.WriteLine("No tours currently assigned.");
        }
    }
}