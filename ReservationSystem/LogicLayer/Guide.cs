using Newtonsoft.Json;
using System.Media;

namespace ReservationSystem;

public class Guide
{
    public Guid GuideId { get; set; }
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
        if (!AllGuides.Select(guide => guide.Name).Contains(name))
            AllGuides.Add(this);
    }

    public static void LoadGuides()
    {
        try
        {
            string jsonGuideAssignmentsPath = TourTools.JsonGuideAssignmentsPath;
            string jsonContent = Program.World.ReadAllText(jsonGuideAssignmentsPath);
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
            Program.World.WriteAllText(jsonGuideAssignmentsPath, updatedJsonContent);
        }
        catch (FileNotFoundException)
        {
            throw new FileNotFoundException("The guide assignments file does not exist.");
        }

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


    public void ViewPersonalTours()
    {
        var upcomingTour = TourTools.TodaysTours
            .Where(t => !t.Started && !t.Deleted && t.AssignedGuide.Name == Name && t.StartTime > Program.World.Now)
            .OrderBy(t => t.StartTime)
            .FirstOrDefault();

        if (AssignedTourIds.Count == 0 || upcomingTour == null)
        {
            GuideHasNoMoreTours.Show();
            return;
        }

        Program.World.WriteLine($"Tours for {Name}:\n");

        int tourNumber = 1;
        foreach (var tourId in AssignedTourIds)
        {
            var tour = TourTools.TodaysTours.FirstOrDefault(t => t.TourId == tourId);
            bool tourInFuture = DateTime.Compare(tour.StartTime, Program.World.Now) == 1;
            if (tour != null && tourInFuture && tour.Started == false)
            {
                string formattedTime = tour.StartTime.ToString("HH:mm");
                string date = DateOnly.FromDateTime(tour.StartTime).ToString();
                Program.World.WriteLine($"Tour {tourNumber} | Date: {date} | Start Time: {formattedTime} | {tour.ExpectedVisitors.Count} reservations");

                tourNumber++;
            }

        }
    }


    public static void ReassignGuideToTour()
    {
        var sortedTours = TourTools.TodaysTours
        .Where(tour => !tour.Started && !tour.Deleted && tour.StartTime > Program.World.Now)
        .OrderBy(tour => tour.StartTime)
        .ToList();
        try { Console.Clear(); } catch { };
        Program.World.WriteLine("Select the tour you want to reassign the guide for: \n");
        for (int i = 0; i < sortedTours.Count; i++)
        {
            string formattedStartTime = sortedTours[i].StartTime.ToString("HH:mm");
            ColourText.WriteColored($"", $"{i + 1} | ", ConsoleColor.Cyan);
            Program.World.Write($"{sortedTours[i].StartTime.ToShortDateString()} | Start Time: ");
            ColourText.WriteColored("", formattedStartTime, ConsoleColor.Cyan);
            Program.World.WriteLine($" | currently assigned to {sortedTours[i].AssignedGuide?.Name ?? "No Guide"}");
        }

        ColourText.WriteColored("\nEnter the ", "number", ConsoleColor.Cyan, " left of the tour to reassign: ");

        int tourIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (tourIndex < 0 || tourIndex >= TourTools.TodaysTours.Count)
        {
            Program.World.WriteLine("Invalid tour selection.");
            return;
        }

        // Display guides for selection from the static list in Guide class
        Program.World.WriteLine("\nSelect a guide to assign:\n");
        var distinctGuides = AllGuides
        .GroupBy(guide => guide.Name)
        .Select(group => group.First())
        .ToList();

        for (int i = 0; i < distinctGuides.Count; i++)
        {
            ColourText.WriteColored($"", $"{i + 1} | ", ConsoleColor.Cyan);
            Program.World.WriteLine($"{distinctGuides[i].Name}");
        }


        ColourText.WriteColored("\nEnter the ", "number", ConsoleColor.Cyan, " of the guide to assign: ");

        int guideIndex = Convert.ToInt32(Console.ReadLine()) - 1;

        if (guideIndex < 0 || guideIndex >= Guide.AllGuides.Count)
        {
            Program.World.WriteLine("Invalid guide selection.");
            return;
        }

        sortedTours[tourIndex].AssignedGuide = Guide.AllGuides[guideIndex];
        TourDataManager.SaveTours();

        try { Console.Clear(); } catch { }
        Program.World.Write("Guide ");
        ColourText.WriteColored("", Guide.AllGuides[guideIndex].Name, ConsoleColor.Blue);
        Program.World.Write(" has been successfully assigned to the tour at ");
        ColourText.WriteColored("", sortedTours[tourIndex].StartTime.ToString("HH:mm"), ConsoleColor.Cyan);
        Program.World.WriteLine(" o'clock.");

        Program.World.Write("\nPress ");
        ColourText.WriteColored("", "Enter", ConsoleColor.Cyan, " to return to Admin Menu or ");
        ColourText.WriteColored("", "Space", ConsoleColor.Cyan, " to return to Ticket Scanner");
        Program.World.WriteLine("");


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
        var availableGuideTours = TourDataManager.FilterByLambda(tour => tour.AssignedGuide.Name == this.Name
            && !tour.Started && !tour.Deleted
            && tour.StartTime > Program.World.Now)
            .OrderBy(tour => tour.StartTime).ToList();

        if (availableGuideTours.Count == 0)  // there are no more availble tours for visitor
        {
            NoAvailbleTour.Show();
            return null;  // break out of void to prefent program crash
        }

        Tour target = availableGuideTours.First();  // the target will chase down the closest next tour to overwrite // Data with visitor added to PresentVisitor will overwrite the target
        target.PresentVisitors.Add(visitor);
        Tour overwite = target;

        // loop trought todays tours and overwite. 
        for (int index = 0; index < TourTools.TodaysTours.Count; index++)
        {
            var tour = TourTools.TodaysTours[index];
            if (tour.TourId == target.TourId && tour.AssignedGuide.Name == this.Name)
            {
                TourTools.TodaysTours[index] = overwite;  // update the Tour with visitor added to Pressent Visitor
                TourDataManager.SaveTours();  // overwrite JSON with the visitor added to Tour 
                return tour;  // break out the method and send tourDetail to display aditional info for guide.
            }
        }
        ForEachError.Show();
        // this is a failsave message, this should not happen unless there is a bug,
        // program should continue without visitor being added to tour.
        return null;
    }
}
