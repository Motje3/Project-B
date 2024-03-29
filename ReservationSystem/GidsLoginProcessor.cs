using Newtonsoft.Json;

public class GidsLoginProcessor
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public void ProcessLoginForm()
    {
        bool isAuthenticated = false;

        // Login loop
        while (!isAuthenticated)
        {
            Console.WriteLine("\nGuide login\n");

            Console.Write("Enter username: ");
            string username = Console.ReadLine();
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            isAuthenticated = AuthenticateUser(username, password);

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

    private bool DisplayMainMenu()
    {
        Console.WriteLine("\nWhat would you like to do?\n");
        Console.WriteLine("1. See personal tours");
        Console.WriteLine("2. Check attending participants");
        Console.WriteLine("3. Add last minute participants");
        Console.WriteLine("4. Logout");

        int guideChoice;
        if (int.TryParse(Console.ReadLine(), out guideChoice))
        {
            switch (guideChoice)
            {
                case 1:
                    DisplayTimetable();
                    break;
                case 2:
                    NoteParticipants();
                    break;
                case 3:
                    AddLastMinuteParticipants();
                    break;
                case 4:
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

    private void DisplayTimetable()
    {
        bool backToMenu = false;

        while (!backToMenu)
        {
            Console.WriteLine("TIMETABLE HAS TO BE SHOWN TO THE GUIDE");
            Console.WriteLine("M: Go back to main menu");

            string input = Console.ReadLine();
            if (input.ToUpper() == "M")
            {
                backToMenu = true;
            }
            else
            {
                Console.WriteLine("Invalid input. Please press M/m to go back to the main menu");
            }
        }
    }

    private void AddLastMinuteParticipants()
    {
        bool backToMenu = false;

        while (!backToMenu)
        {
            Console.WriteLine("Adding last minute participants");
            // Here you could add functionality to add participants
            Console.WriteLine("M: Go back to main menu");

            if (Console.ReadLine().ToUpper() == "M")
            {
                backToMenu = true;
            }
        }
    }

        private void NoteParticipants()
    {
        int hour;
        Console.WriteLine("Which hour's tour would you like to check participants for? (9-17)");
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out hour) && hour >= 9 && hour <= 17)
            {
                break; // hour is valid, exit the loop
            }
            else
            {
                Console.WriteLine("Invalid hour. Please enter a number between 9 and 17.");
            }
        }

        string filePath = "./JSON-Files/guidedTours.json";
        var tours = LoadToursFromFile(filePath);

        if (tours.ContainsKey(hour))
        {
            List<Visitor> presentVisitors = new List<Visitor>();
            foreach (var visitor in tours[hour])
            {
                Console.Write($"Is visitor {visitor.Name} present? (Y/N): ");
                string response = Console.ReadLine().Trim().ToUpper();
                if (response == "Y")
                {
                    presentVisitors.Add(visitor);
                }
            }

            SaveAttendingVisitorsToFile(hour, presentVisitors);
        }
        else
        {
            Console.WriteLine("No visitors are scheduled for this hour.");
        }

        Console.WriteLine("M: Go back to main menu");
        while (Console.ReadLine().ToUpper() != "M")
        {
            Console.WriteLine("Invalid input. Please press M/m to go back to the main menu");
        }
    }

    private Dictionary<int, List<Visitor>> LoadToursFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<int, List<Visitor>>>(json);
        }
        return new Dictionary<int, List<Visitor>>();
    }

    private void SaveAttendingVisitorsToFile(int hour, List<Visitor> presentVisitors)
    {
        // Check if there are no present visitors.
        if (presentVisitors.Count == 0)
        {
            Console.WriteLine($"No visitors attending the {hour}:00 tour. No checklist was saved.");
            return; // Exit the method early.
        }

        var checklist = new Dictionary<int, List<Visitor>>
        {
            { hour, presentVisitors }
        };

        string filePath = "./JSON-Files/ChecklistGuide.json";
        string json = JsonConvert.SerializeObject(checklist, Formatting.Indented);
        File.WriteAllText(filePath, json);

        Console.WriteLine("Checklist for attending visitors saved successfully.");
    }

    private bool AuthenticateUser(string username, string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Username == username && cred.Password == password);
    }

    private List<Credential> LoadUserCredentials()
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