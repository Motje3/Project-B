using Newtonsoft.Json;

public class Guide
{
    public Guid GuideId { get; private set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Guid> AssignedTourIds { get; set; } = new List<Guid>();
    public static List<Guide> AllGuides = new List<Guide>();
    

    public Guide(string name, Guid tourId)
    {
        GuideId = Guid.NewGuid();
        Name = name;
        AssignedTourIds.Add(tourId); // Add the tourId to the list
    }


    public Guide(string name)
    {
        GuideId = Guid.NewGuid();
        Name = name;
        AllGuides.Add(this);
    }

    public void AssignTour(Guid tourId)
    {
        if (!AssignedTourIds.Contains(tourId))
        {
            AssignedTourIds.Add(tourId);
        }
    }



    [JsonConstructor]
    public Guide(Guid guideId, string name, Guid assignedTourId)
    {
        GuideId = guideId;
        Name = name;
        AssignedTourIds = AssignedTourIds;
    }

    public static void LoadGuides()
    {
        List<dynamic> guideAssignments = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(Tour.JsonGuideAssignmentsPath));
        foreach (var guideEntry in guideAssignments)
        {
            string guideName = guideEntry.GuideName;
            if (!Guide.AllGuides.Any(g => g.Name == guideName))
            {
                new Guide(guideName);  // This will automatically add the guide to the AllGuides list due to the constructor logic
            }
        }
    }


    public void CheckInVisitor(Visitor visitor)
    {
        //to be implemented
    }

    public void CompleteTour()
    {
        //to be implemented
    }

    public static void ReassignGuideToTour()
    {
        Console.WriteLine("Select a tour to reassign a guide:");
        for (int i = 0; i < Tour.TodaysTours.Count; i++)
        {
            Console.WriteLine($"{i + 1}. Tour at {Tour.TodaysTours[i].StartTime} currently assigned to {Tour.TodaysTours[i].AssignedGuide?.Name ?? "No Guide"}");
        }

        Console.Write("Enter the number of the tour to reassign: ");
        int tourIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (tourIndex < 0 || tourIndex >= Tour.TodaysTours.Count)
        {
            Console.WriteLine("Invalid tour selection.");
            return;
        }

        // Display guides for selection from the static list in Guide class
        Console.WriteLine("Select a guide to assign:");
        for (int i = 0; i < Guide.AllGuides.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {Guide.AllGuides[i].Name}");
        }

        Console.Write("Enter the number of the guide to assign: ");
        int guideIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (guideIndex < 0 || guideIndex >= Guide.AllGuides.Count)
        {
            Console.WriteLine("Invalid guide selection.");
            return;
        }

        // Assign the selected guide from the existing list
        Tour.TodaysTours[tourIndex].AssignedGuide = Guide.AllGuides[guideIndex];

        // Save changes
        Tour.SaveTours();

        try { Console.Clear(); } catch { }
        Console.WriteLine($"Guide {Guide.AllGuides[guideIndex].Name} has been successfully assigned to the tour at {Tour.TodaysTours[tourIndex].StartTime:hh:mm tt} o'clock.");
        Thread.Sleep(2000);
        try { Console.Clear(); } catch { }
    }
}