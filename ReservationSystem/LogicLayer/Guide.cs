using Newtonsoft.Json;

public class Guide
{
    public Guid GuideId { get; private set; }
    public string Name { get; set; }
    [JsonIgnore]
    public List<Guid> AssignedTourIds { get; set; } = new List<Guid>();
    public static List<Guide> AllGuides = new List<Guide>();

    public Guide(string name, Guid guideId)
    {
        GuideId = guideId;
        Name = name;
        AllGuides.Add(this);
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
    public Guide(Guid guideId, string name, List<Guid> assignedTourIds)
    {
        GuideId = guideId;
        Name = name;
        AssignedTourIds = assignedTourIds ?? new List<Guid>();
        AllGuides.Add(this);
    }

    public static void LoadGuides()
    {
        string jsonGuideAssignmentsPath = Tour.JsonGuideAssignmentsPath;
        string jsonContent = File.ReadAllText(jsonGuideAssignmentsPath);
        List<dynamic> guideAssignments = JsonConvert.DeserializeObject<List<dynamic>>(jsonContent);

        foreach (var guideEntry in guideAssignments)
        {
            string guideName = guideEntry.GuideName;
            string guideIdStr = guideEntry.GuideId;
            Guid guideId;

            // Generate a new GUID if GuideId is empty or whitespace
            if (string.IsNullOrWhiteSpace(guideIdStr))
            {
                guideId = Guid.NewGuid();
                guideEntry.GuideId = guideId.ToString();
            }
            else
            {
                guideId = Guid.Parse(guideIdStr);
            }

            // Check if the guide already exists in the AllGuides list
            if (!Guide.AllGuides.Any(g => g.Name == guideName))
            {
                new Guide(guideName, guideId);
            }
            else
            {
                var existingGuide = Guide.AllGuides.First(g => g.Name == guideName);
                existingGuide.GuideId = guideId;
            }
        }

        // Serialize the updated guide assignments back to the JSON file
        string updatedJsonContent = JsonConvert.SerializeObject(guideAssignments, Formatting.Indented);
        File.WriteAllText(jsonGuideAssignmentsPath, updatedJsonContent);
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
        int tourIndex;
        while (true)
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out tourIndex))
            {
                tourIndex--; // Verminder de invoer met 1 om de juiste index te krijgen
                if (tourIndex >= 0 && tourIndex < Tour.TodaysTours.Count)
                {
                    break; // Geldige invoer, breek de lus
                }
                else
                {
                    Console.WriteLine("Invalid tour selection. Please enter a valid tour number.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
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
