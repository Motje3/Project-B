using Newtonsoft.Json;

public static class GidsLoginProcessor
{
    public class Credential
    {
        public string Password { get; set; }
    }

    private static Guide _myGuide;
    private static GuidedTour _myTour;

    public static void ProcessLoginForm(string guideCode)
    {
        _loadMyGuide(guideCode);
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
                Console.WriteLine("\nAccess Denied, invalid password. Returning to start menu\n");
                // Go back to start menu
                Thread.Sleep(1000*3);
                ReservationManager.ValidateCodeAndProcessReservations();
            }
        }

        // Go back to start menu if the guide doesn't have any tours
        if (_myGuide == null)
        {
            Console.WriteLine("You currently don't have any tours");
            Console.WriteLine("You will be redirected to start menu in 4 seconds");
            Thread.Sleep(1000*4);
            //ReservationManager.ValidateCodeAndProcessReservations();
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
        if (_myTour.ExpectedVisitors.Count == 0)
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | Nobody has made a reservation\n");
        else if (_myTour.ExpectedVisitors.Count == 1)
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | {_myTour.ExpectedVisitors.Count} visitor has made a resevertaion\n");
        else
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | {_myTour.ExpectedVisitors.Count} visitors have made a resevertaion\n");
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("1. See personal tours");
        Console.WriteLine("2. Check attending participants");
        Console.WriteLine("3. Logout");

        string guideChoice = Console.ReadLine();
        switch (guideChoice)
        {
            case "1":
                //DisplayTimetable();
                break;
            case "2":
                NoteParticipants();
                break;
            case "3":
                Console.WriteLine("Logging out...");
                return false; // Stops the main menu loop and logs out
            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                break;
        }

        return true; // Continues the main menu loop
    }

    private static void NoteParticipants()
    {
        List<string> addedCodes = new();
        if (_myGuide == null)
        {
            return;
        }

        string visitorCode;
        do
        {
            Console.WriteLine($"Currently added tickets: [{_returnListAsString(addedCodes)}]");
            // Guide has to scan a ticket of a visitor (Already checks if code is in expected visitors)
            visitorCode = _askVisitorCode();
            if (visitorCode != "stop" && !addedCodes.Contains(visitorCode))
                addedCodes.Add(visitorCode);
        }
        while (visitorCode != "stop");


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

    // Asks the guide for a visitor code and checks if the code is in expectedVisitors
    public static string _askVisitorCode()
    {
        List<string> allowedCodes = new();
        foreach (Visitor currentVisitor in _myTour.ExpectedVisitors)
        {
            allowedCodes.Add(currentVisitor.TicketCode);
        }

        Console.WriteLine("Please scan the ticket of a visitor or type in \"stop\" to stop checking in visitors");
        string visitorTicket = Console.ReadLine();
        if (visitorTicket == "stop")
            return "stop";
        else if (allowedCodes.Contains(visitorTicket))
            return visitorTicket;
        do
        {
            Console.WriteLine("This visitor doesn't have reservation for this tour,");
            Console.WriteLine("Try again or try another visitor");
            visitorTicket = Console.ReadLine();
        } 
        while (!(allowedCodes.Contains(visitorTicket) || (visitorTicket == "stop")));

        return visitorTicket;
    }

    private static void _loadMyGuide(string guideCode)
    {
        Guide returnGuide = null;

        foreach (GuidedTour tour in GuidedTour.CurrentTours) 
        {
            if (tour.AssignedGuide == null)
                {

                }
            else if (tour.AssignedGuide.TicketCode == guideCode)
            {
                returnGuide = tour.AssignedGuide;
                _myTour = GuidedTour.FindTourById(returnGuide.AssingedTourId);
                break;
            }
        }

        _myGuide = returnGuide;
    }

    private static string _returnListAsString(List<string> list)
    {
        string retStr = "";
        if (list.Count == 0)
            return retStr;

        foreach (string str in list)
        {
            retStr += $"{str}, ";
        }
        retStr = retStr.Remove(retStr.Length - 2);
        return retStr;
    }
}