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
            Console.WriteLine("\nGuide login");
            Console.WriteLine("Enter password: ");
            string password = Console.ReadLine();

            isAuthenticated = AuthenticateUser(password);

            if (isAuthenticated)
            {
                Console.WriteLine("\nAccess Granted!\n");
                Thread.Sleep(1000 * 1);
                try{Console.Clear();}catch{} 
            }
            else
            {
                Console.WriteLine("\nAccess Denied, invalid password. Returning to start menu in 3 seconds\n");
                // Go back to start menu
                Thread.Sleep(1000*3);
                ReservationManager.ValidateCodeAndProcessReservations();
            }
        }

        // Go back to start menu if the guide doesn't have any tours
        if (_myGuide == null)
        {
            Console.WriteLine("You currently don't have any tours");
            Console.WriteLine("You will be redirected to start menu in 3 seconds");
            Thread.Sleep(1000*3);
            ReservationManager.ValidateCodeAndProcessReservations();
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
        // Printing next tour of the guide
        if (_myTour.ExpectedVisitors.Count == 0)
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | Nobody has made a reservation\n");
        else if (_myTour.ExpectedVisitors.Count == 1)
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | {_myTour.ExpectedVisitors.Count} visitor has made a resevertaion\n");
        else
            Console.WriteLine($"Your next tour is: {DateOnly.FromDateTime(_myTour.StartTime)} | {TimeOnly.FromDateTime(_myTour.StartTime)} - {TimeOnly.FromDateTime(_myTour.EndTime)} | {_myTour.ExpectedVisitors.Count} visitors have made a resevertaion\n");
        
        // Printing menu options
        Console.WriteLine("What would you like to do?\n");
        Console.WriteLine("1. See personal tours");
        Console.WriteLine("2. Check in attending visitors for your next tour");
        Console.WriteLine("3. Log out");

        string guideChoice = Console.ReadLine();
        Console.WriteLine("");

        switch (guideChoice)
        {
            case "1":
                ShowGuideTours();
                break;
            case "2":
                NoteVisitors();
                break;
            case "3":
                Console.WriteLine("Logging out...");
                Thread.Sleep(1000 * 2);
                return false; // Stops the main menu loop and logs out
            default:
                Console.WriteLine("Invalid choice. Please enter a valid option.");
                break;
        }

        return true; // Continues the main menu loop
    }

    private static void NoteVisitors()
    {
        List<string> addedCodes = new();
        // Add visitor code already checked in to list
        foreach (Visitor currentVisitor in _myTour.PresentVisitors)
        {
            addedCodes.Add(currentVisitor.TicketCode);
        }

        if (_myGuide == null)
        {
            return;
        }

        string visitorCode;
        do
        {
            Console.WriteLine($"Currently checked in tickets: [{_returnListAsString(addedCodes)}]");
            // Guide has to scan a ticket of a visitor (Already checks if code is in expected visitors)
            visitorCode = _askVisitorCode();
            Visitor visitor = Visitor.FindVisitorByTicketCode(visitorCode);
            if (visitorCode != "stop" && !addedCodes.Contains(visitorCode))
            {
                addedCodes.Add(visitorCode);
                _myGuide.CheckInVisitor(visitor);
            }
        }
        while (visitorCode != "stop");

        try{Console.Clear();}catch{} 
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

    // Displays all tours where _myGuide is the guide
    private static void ShowGuideTours()
    {
        try{Console.Clear();}catch{} 
        int guideToursIndex = 1;
        foreach(GuidedTour tour in GuidedTour.CurrentTours)
        {
            if (tour.AssignedGuide == null)
            {

            }
            else if (tour.AssignedGuide.TicketCode == _myGuide.TicketCode)
            {
                Console.WriteLine($"{guideToursIndex} | {DateOnly.FromDateTime(tour.StartTime)} | {TimeOnly.FromDateTime(tour.StartTime)} - {TimeOnly.FromDateTime(tour.EndTime)} | {tour.ExpectedVisitors.Count} reservations ");
                guideToursIndex++;
            }
        }
        Console.WriteLine("");
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

    // Sets guide with given guideCode to _myGuide and sets _myTour to GuidedTour that corresponds to _myGuide.AssingedTourId    
    private static void _loadMyGuide(string guideCode)
    {
        Guide returnGuide = null;

        foreach (GuidedTour tour in GuidedTour.CurrentTours) 
        {
            if (tour.AssignedGuide == null){}                
            else if (tour.AssignedGuide.TicketCode == guideCode)
            {
                returnGuide = tour.AssignedGuide;
                _myTour = GuidedTour.FindTourById(returnGuide.AssingedTourId);
                break;
            }
        }
        _myGuide = returnGuide;
    }

    // Returns the given list of strings as a string where each element is seperated by ", ", used in NoteVisitors
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