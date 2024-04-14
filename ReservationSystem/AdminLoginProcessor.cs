using Newtonsoft.Json;

public static class AdminLoginProcessor
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    // To be changed
    public static void ProcessLoginForm()
    {
        Console.WriteLine("\nAdmin login\n");

        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (AuthenticateUser(username, password))
        {
            Console.WriteLine("\nAccess Granted!\n");
            ShowAdminMenu();
        }
        else
        {
            Console.WriteLine("\nAccess Denied. Invalid username or password.\n");
        }
    }

    private static bool AuthenticateUser(string username, string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Username == username && cred.Password == password);
    }

    private static List<Credential> LoadUserCredentials()
    {
        string filePath = "./JSON-Files/AdminCredentials.json";
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

    // To be changed
    public static void ShowAdminMenu()
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            Console.WriteLine("\nAdmin Menu:");
            Console.WriteLine("1. Change capacity of a tour");
            Console.WriteLine("2. Change time of a tour");
            Console.WriteLine("3. View Tours");;
            Console.WriteLine("4. Exit");

            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    // To be implemented
                    ChangeTourCapacity();
                    break;
                case "2":
                    // To be implemented
                    ChangeTourTime();
                    break;
                case "3":
                    GuidedTour.PrintToursOpenToday();
                    break;
                case "4":
                    continueRunning = false;
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void ChangeTourCapacity()
    {
        // To be implemented, currently low priority in Trello so leave as is until told otherwise
        throw new NotImplementedException();
        /*Console.Write("Enter new capacity: ");
        if (int.TryParse(Console.ReadLine(), out int newCapacity))
        {
            if (guidedTour.UpdateMaxCapacity(newCapacity))
            {
                Console.WriteLine($"Capacity updated to {newCapacity}.");
            }
            else
            {
                Console.WriteLine("Failed to update capacity. Make sure the new capacity is valid and does not conflict with existing tours.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }*/
    }

    private static void ChangeTourTime()
    {
        // To be implemented, currently low priority in Trello so leave as is until told otherwise
        throw new NotImplementedException();
        /*
        Console.Write("Enter the tour hour you would like to cancel: ");
        int oldTourHour = int.Parse(Console.ReadLine());


        if (guidedTour.ChangeTourTime(oldTourHour))
        {
            Console.WriteLine($"Tour at {oldTourHour} ockock has been cancelled");
        }
        else
        {
            Console.WriteLine("Failed to cancel tour.");
        }
        */
    }
}
