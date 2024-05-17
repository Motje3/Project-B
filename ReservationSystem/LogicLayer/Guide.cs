using Newtonsoft.Json;
using ReservationSystem;

public class Guide
{
    public Guid GuideId { get; private set; }
    public string Name { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public List<Guid> AssignedTourIds { get; set; } = new List<Guid>();
    public static List<Guide> AllGuides = new List<Guide>();

    public Guide(string name, Guid guideId, string password)
    {
        GuideId = guideId;
        Name = name;
        Password = password;
        AllGuides.Add(this);
    }

    [JsonConstructor]
    public Guide(Guid guideId, string name, string password, List<Guid> assignedTourIds)
    {
        GuideId = guideId;
        Name = name;
        Password = password;
        AssignedTourIds = assignedTourIds ?? new List<Guid>();
        AllGuides.Add(this);
    }

    public static void LoadGuides()
    {
        string jsonGuideAssignmentsPath = Tour.JsonGuideAssignmentsPath;
        if (!File.Exists(jsonGuideAssignmentsPath))
        {
            throw new FileNotFoundException("The guide assignments file does not exist.");
        }

        string jsonContent = File.ReadAllText(jsonGuideAssignmentsPath);
        List<dynamic> guideAssignments = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

        foreach (var guideEntry in guideAssignments)
        {
            string guideName = guideEntry.GuideName;
            string guideIdStr = guideEntry.GuideId;
            string password = guideEntry.Password;
            Guid guideId;

            if (string.IsNullOrWhiteSpace(guideIdStr))
            {
                guideId = Guid.NewGuid();
                guideEntry.GuideId = guideId.ToString();
            }
            else
            {
                guideId = Guid.Parse(guideIdStr);
            }

            if (!Guide.AllGuides.Any(g => g.Name == guideName))
            {
                new Guide(guideName, guideId, password);
            }
            else
            {
                var existingGuide = Guide.AllGuides.First(g => g.Name == guideName);
                existingGuide.GuideId = guideId;
                existingGuide.Password = password;
            }
        }

        string updatedJsonContent = JsonConvert.SerializeObject(guideAssignments, Formatting.Indented);
        File.WriteAllText(jsonGuideAssignmentsPath, updatedJsonContent);
    }

    public static Guide AuthenticateGuide(string password)
    {
        return AllGuides.FirstOrDefault(g => g.Password == password);
    }

    public void AssignTour(Guid tourId)
    {
        if (!AssignedTourIds.Contains(tourId))
        {
            AssignedTourIds.Add(tourId);

        }
    }


    public static void ViewPersonalTours(Guide guide)
    {
        Console.WriteLine($"Tours for {guide.Name}:\n");

        int tourNumber = 1;
        foreach (var tourId in guide.AssignedTourIds)
        {
            var tour = Tour.TodaysTours.FirstOrDefault(t => t.TourId == tourId);
            if (tour != null)
            {
                string formattedTime = tour.StartTime.ToString("hh:mm tt");
                Console.WriteLine($"Tour {tourNumber} | Start Time: {formattedTime} |");
                tourNumber++;
            }

        }
    }


    public static void ReassignGuideToTour()
    {
        Program.World.WriteLine("Select a tour to reassign a guide:");
        for (int i = 0; i < Tour.TodaysTours.Count; i++)
        {
            Program.World.WriteLine($"{i + 1}. Tour at {Tour.TodaysTours[i].StartTime} currently assigned to {Tour.TodaysTours[i].AssignedGuide?.Name ?? "No Guide"}");
        }

        Program.World.Write("Enter the number of the tour to reassign: ");
        int tourIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (tourIndex < 0 || tourIndex >= Tour.TodaysTours.Count)
        {
            Program.World.WriteLine("Invalid tour selection.");
            return;
        }

        // Display guides for selection from the static list in Guide class
        Program.World.WriteLine("Select a guide to assign:");
        for (int i = 0; i < Guide.AllGuides.Count; i++)
        {
            Program.World.WriteLine($"{i + 1}. {Guide.AllGuides[i].Name}");
        }

        Program.World.Write("Enter the number of the guide to assign: ");

        int guideIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (guideIndex < 0 || guideIndex >= Guide.AllGuides.Count)
        {
            Program.World.WriteLine("Invalid guide selection.");
            return;
        }

        Tour.TodaysTours[tourIndex].AssignedGuide = Guide.AllGuides[guideIndex];
        Tour.SaveTours();

        try { Console.Clear(); } catch { }
        Program.World.WriteLine($"Guide {Guide.AllGuides[guideIndex].Name} has been successfully assigned to the tour at {Tour.TodaysTours[tourIndex].StartTime:hh:mm tt} o'clock.");
        Thread.Sleep(2000);
        try { Console.Clear(); } catch { }
    }
}
