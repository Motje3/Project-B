using ReservationSystem;

public class GuideLoginMenu : View
{
    public static void ProcessLoginForm(string userCode)
    {
        Console.WriteLine("\nGuide login\n");
        
        Console.Write("Enter password: ");
        string password = AdminBackEnd.ReadPassword();


        if (GuideMenuBackEnd.AuthenticateGuide(password))
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
            Console.WriteLine("\nGuide Menu:");
            Console.WriteLine("1. View personal tours(To Be Implemented)");
            Console.WriteLine("2. Exit");

            Write("\nEnter your choice: ");
            string choice = ReadLine();

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
                    WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    
}