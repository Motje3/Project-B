namespace ReservationSystem;

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
        GidsLoginMenu GLM = new GidsLoginMenu();
        GidsAccessPassed GAP = new GidsAccessPassed();
        GidsAccessFailed GAF = new GidsAccessFailed();
        GidsNoTour GNT = new GidsNoTour();
        
        _loadMyGuide(guideCode);
        bool isAuthenticated = false;

        // Login loop
        while (!isAuthenticated)
        {
            string password = GLM.Show();

            isAuthenticated = AuthenticateUser(password);

            if (isAuthenticated)
            {
                GAP.Show();
                
                Thread.Sleep(1000 * 1);
                try{Console.Clear();}catch{} 
            }
            else
            {
                GAF.Show();
                // Go back to start menu
                Thread.Sleep(1000*3);
                ReservationManager.ValidateCodeAndProcessReservations();
            }
        }

        // Go back to start menu if the guide doesn't have any tours
        if (_myGuide == null)
        {
            GNT.Show();
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
        GidsDisplayTourInfo DTI = new GidsDisplayTourInfo();
        // DTI requires felllowing parameters:
        // DateOnly.FromDateTime(_myTour.StartTime)
        // TimeOnly.FromDateTime(_myTour.StartTime)}
        // TimeOnly.FromDateTime(_myTour.EndTime)}
        // _myTour.ExpectedVisitors.Count
        LogOut LO = new LogOut();
        
        {
            if (_myTour.ExpectedVisitors.Count == 0)
                DTI.ShowEmpty(DateOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.EndTime), _myTour.ExpectedVisitors.Count);
            else if (_myTour.ExpectedVisitors.Count == 1)
                DTI.ShowOne(DateOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.EndTime), _myTour.ExpectedVisitors.Count);
            else
                DTI.ShowMany(DateOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.StartTime), TimeOnly.FromDateTime(_myTour.EndTime), _myTour.ExpectedVisitors.Count);
            
            // Printing menu options'
            GidsMainMenu GMM = new GidsMainMenu();
            string guideChoice = GMM.Show();
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
                    LO.Show();
                    Thread.Sleep(1000 * 2);
                    return false; // Stops the main menu loop and logs out
                default:
                    
                    break;
            }

            return true; // Continues the main menu loop
        }
    }

    private static void NoteVisitors()
    {
        CheckIn CI = new CheckIn();
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
            string Code = _returnListAsString(addedCodes);
            CI.Show(Code);
            // Guide has to scan a ticket of a visitor (Already checks if code is in expected visitors)
            visitorCode = _askVisitorCode();
            Visitor visitor = Visitor.FindVisitorByTicketCode(visitorCode);
            if (visitorCode != "stop" && !addedCodes.Contains(visitorCode))
            {
                addedCodes.Add(visitorCode);
                //_myGuide.CheckInVisitor(visitor);
            }
        }
        while (visitorCode != "stop");

        try{Console.Clear();}catch{} 
    }

    // Asks the guide for a visitor code and checks if the code is in expectedVisitors
    public static string _askVisitorCode()
    {
        ScanTicket ST = new ScanTicket();
        ScanTicketFailed ST_F = new ScanTicketFailed();

        List<string> allowedCodes = new();
        foreach (Visitor currentVisitor in _myTour.ExpectedVisitors)
        {
            allowedCodes.Add(currentVisitor.TicketCode);
        }
        string visitorTicket = ST.Show();
        if (visitorTicket == "stop")
            return "stop";
        else if (allowedCodes.Contains(visitorTicket))
            return visitorTicket;
        do
        {
            visitorTicket = ST_F.Show();
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
            JSON_NotFound  JSON_NF = new JSON_NotFound();
            JSON_NF.Show("GidsCredentials.json");
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
