using System.Runtime;
using Newtonsoft.Json;
using ReservationSystem;
namespace ReservationSystem;

public class Guide
{
    public Guid GuideId { get; private set; }
    public string Name { get; set; }
    public string Password { get; set; }
    [JsonIgnore]
    public List<Guid> AssignedTourIds { get; set; } = new List<Guid>();
    public static List<Guide> AllGuides = new List<Guide>();

    public Guide(Guid guideId, string name, string password)
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
                new Guide(guideId, guideName, password);
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
        Program.World.WriteLine($"Tours for {guide.Name}:\n");

        int tourNumber = 1;
        foreach (var tourId in guide.AssignedTourIds)
        {
            var tour = Tour.TodaysTours.FirstOrDefault(t => t.TourId == tourId);
            if (tour != null)
            {
                string formattedTime = tour.StartTime.ToString("HH:mm");
                Program.World.WriteLine($"Tour {tourNumber} | Start Time: {formattedTime} ");

                tourNumber++;
            }

        }
    }


    public static void ReassignGuideToTour()
    {
        var sortedTours = Tour.TodaysTours
        .Where(tour => !tour.Completed && !tour.Deleted)
        .OrderBy(tour => tour.StartTime)
        .ToList();
        Console.Clear();
        Program.World.WriteLine("Select the tour you want to reassign the guide for: \n");
        for (int i = 0; i < sortedTours.Count; i++)
        {
            string formattedStartTime = Tour.TodaysTours[i].StartTime.ToString("HH:mm");
            ColourText.WriteColored($"{i + 1}", " | ", ConsoleColor.Cyan);
            Console.Write($"{Tour.TodaysTours[i].StartTime.ToShortDateString()} | Start Time: ");
            ColourText.WriteColored("", formattedStartTime, ConsoleColor.Cyan);
            Console.WriteLine($" | currently assigned to {Tour.TodaysTours[i].AssignedGuide?.Name ?? "No Guide"}");
        }

        ColourText.WriteColored("\nEnter the ", "number", ConsoleColor.Cyan, " left of the tour to reassign: ");

        int tourIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (tourIndex < 0 || tourIndex >= Tour.TodaysTours.Count)
        {
            Program.World.WriteLine("Invalid tour selection.");
            return;
        }

        // Display guides for selection from the static list in Guide class
        Program.World.WriteLine("Select a guide to assign:");
        var distinctGuides = Guide.AllGuides
        .GroupBy(guide => guide.Name)
        .Select(group => group.First())
        .ToList();

        for (int i = 0; i < distinctGuides.Count; i++)
        {
            ColourText.WriteColored($"{i + 1}", " | ", ConsoleColor.Cyan);
            Console.WriteLine($"{distinctGuides[i].Name}");
        }


        ColourText.WriteColored("Enter the ", "number", ConsoleColor.Cyan, " of the guide to assign: ");

        int guideIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (guideIndex < 0 || guideIndex >= Guide.AllGuides.Count)
        {
            Program.World.WriteLine("Invalid guide selection.");
            return;
        }

        Tour.TodaysTours[tourIndex].AssignedGuide = Guide.AllGuides[guideIndex];
        Tour.SaveTours();

        try { Console.Clear(); } catch { }
        Program.World.Write("Guide ");
        ColourText.WriteColored("", Guide.AllGuides[guideIndex].Name, ConsoleColor.Blue);
        Program.World.Write(" has been successfully assigned to the tour at ");
        ColourText.WriteColored("", Tour.TodaysTours[tourIndex].StartTime.ToString("HH:mm"), ConsoleColor.Cyan);
        Program.World.WriteLine(" o'clock.");

        Program.World.Write("Press ");
        ColourText.WriteColored("", "Enter", ConsoleColor.Cyan, " to return to Admin Menu or ");
        ColourText.WriteColored("", "Space", ConsoleColor.Cyan, " to close it.");
        Console.WriteLine();


        ConsoleKey key;
        do
        {
            key = Console.ReadKey(true).Key;
        } while (key != ConsoleKey.Enter && key != ConsoleKey.Spacebar);

        if (key == ConsoleKey.Spacebar)
        {
            try { Console.Clear(); } catch { }
            // ShowAdminMenu(); // Replace this with the method that shows the Admin Menu
        }

    }

    public Tour AddVisitorLastMinute(Visitor visitor)
    {
        // refresh TodaysTours data.
        // ussing method to filter specific conditions.
        // orderd by starttime.
        var availableGuideTours = Tour.FilterByLambda(tour => tour.AssignedGuide.Name == this.Name
            && !tour.Completed && !tour.Deleted 
            && tour.ExpectedVisitors.Count < tour.MaxCapacity
            && tour.StartTime > DateTime.Now)
            .OrderBy(tour => tour.StartTime).ToList(); 
        
        if (availableGuideTours.Count == 0)  // there are no more availble tours for visitor
        {
            NoAvailbleTour.Show();
            return null;  // break out of void to prefent program crash
        }
        
        Tour target = availableGuideTours.First();  // the target will chase down the closest next tour to overwrite // Data with visitor added to PresentVisitor will overwrite the target
        target.ExpectedVisitors.Add(visitor);
        Tour overwite = target;

        // loop trought todays tours and overwite. 
        for (int index = 0; index < Tour.TodaysTours.Count; index++)
        {
            var tour = Tour.TodaysTours[index];
            if (tour.TourId == target.TourId && tour.AssignedGuide.Name == this.Name)  
            {
                Tour.TodaysTours[index] = overwite;  // update the Tour with visitor added to Pressent Visitor
                Tour.SaveTours();  // overwrite JSON with the visitor added to Tour 
                return tour;  // break out the method and send tourDetail to display aditional info for guide.
            }
        }
        ForEachError.Show();
        // this is a failsave message, this should not happen unless there is a bug,
        // program should continue without visitor being added to tour.
        return null;
    }
}
