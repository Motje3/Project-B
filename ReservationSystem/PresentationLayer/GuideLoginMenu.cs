using Newtonsoft.Json;

public static class GuideLoginMenu
{
    public static void ProcessLoginForm(string userCode)
    {
        Console.WriteLine("\nGuide login\n");
        
        Console.Write("Enter password: ");
        string password = Console.ReadLine();


        if (GuideMenuBackEnd.AuthenticateGuide(password))
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
                    //show guidedtours
                    break;
                case "2":
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