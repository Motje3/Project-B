using Newtonsoft.Json;

public static class GidsLoginProcessor
{
    public class Credential
    {
        public string Password { get; set; }
    }

    public static void ProcessLoginForm()
    {
        bool isAuthenticated = false;

        // Login loop
        while (!isAuthenticated)
        {
            Console.WriteLine("\nGuide login\n");
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            isAuthenticated = AuthenticateUser(password);

            if (isAuthenticated)
            {
                Console.WriteLine("\nAccess Granted!\n");
            }
            else
            {
                Console.WriteLine("\nAccess Denied. Invalid username or password.\n");
            }
        }

        // After successful login
        bool continueRunning = true;
        while (continueRunning)
        {
            continueRunning = DisplayMainMenu();
        }
    }

    private static bool DisplayMainMenu()
    {
        Console.WriteLine("\nWhat would you like to do?\n");
        Console.WriteLine("1. See personal tours");
        Console.WriteLine("2. Check attending participants");
        Console.WriteLine("3. Logout");

        int guideChoice;
        if (int.TryParse(Console.ReadLine(), out guideChoice))
        {
            switch (guideChoice)
            {
                case 1:
                    //DisplayTimetable();
                    break;
                case 2:
                    //NoteParticipants();
                    break;
                case 3:
                    Console.WriteLine("Logging out...");
                    return false; // Stops the main menu loop and logs out
                default:
                    Console.WriteLine("Invalid choice. Please enter a valid option.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a number.");
        }

        return true; // Continues the main menu loop
    }

    private static bool AuthenticateUser(string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Password == password);
    }

    private static List<Credential> LoadUserCredentials()
    {
        string filePath = "./JSON-Files/GidsCredentials.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Credential>>(jsonData) ?? new List<Credential>();
        }
        else
        {
            Console.WriteLine("Credential file not found.");
            return new List<Credential>();
        }
    }
}