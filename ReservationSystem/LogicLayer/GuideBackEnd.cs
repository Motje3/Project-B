using Newtonsoft.Json;

public static class GuideBackEnd
{
    public class Credential
    {
        public string Password { get; set; }
    }

    private static Guide _myGuide;
    private static GuidedTour _myTour;

    public static void LoadMyGuide(string guideCode)
    {
        // Correctly extracting the Guide from the first matching GuidedTour
        var matchingTour = GuidedTour.CurrentTours
            .FirstOrDefault(tour => tour.AssignedGuide?.TicketCode == guideCode);

        if (matchingTour != null && matchingTour.AssignedGuide != null)
        {
            _myGuide = matchingTour.AssignedGuide;
            _myTour = GuidedTour.FindTourById(_myGuide.AssingedTourId);
        }
        else
        {
            _myGuide = null;
            _myTour = null;
        }
    }

    public static bool AuthenticateGuide(string password)
    {
        try
        {
            List<Credential> credentials = LoadUserCredentials();
            return credentials.Any(cred => cred.Password == password);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error authenticating guide: {ex.Message}");
            return false;
        }
    }

    public static List<GuidedTour> GetGuideTours()
    {
        return _myGuide != null ? GuidedTour.CurrentTours.Where(tour => tour.AssignedGuide?.TicketCode == _myGuide.TicketCode).ToList() : new List<GuidedTour>();
    }

    private static List<Credential> LoadUserCredentials()
    {
        string filePath = "./DataLayer/JSON-Files/GidsCredentials.json";
        try
        {
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
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to load credentials: {ex.Message}");
            return new List<Credential>();
        }
    }
}