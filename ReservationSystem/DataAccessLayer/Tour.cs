using Newtonsoft.Json;
using ReservationSystem;

public class Tour
{
    public Guid TourId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int Duration { get; private set; }
    public int MaxCapacity { get; private set; }
    public List<Visitor> ExpectedVisitors { get; set; } = new List<Visitor>();
    public List<Visitor> PresentVisitors { get; set; } = new List<Visitor>();
    public bool Completed { get; set; }
    public bool Deleted { get; set; }
    public Guide AssignedGuide { get; set; }

    public static string JsonFilePath => $"./JSON-Files/Tours-{Program.World.Now:yyyyMMdd}.json";
    public static string JsonTourSettingsPath => $"./JSON-Files/TourSettings.json";
    public static string JsonGuideAssignmentsPath => $"./JSON-Files/GuideAssignments.json";


    public static List<Tour> TodaysTours { get; private set; } = new List<Tour>();

    public Tour(Guid tourId, DateTime startTime, int duration, int maxCapacity, bool completed, bool deleted, Guide assignedGuide)
    {
        TourId = tourId;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(duration);
        Duration = duration;
        MaxCapacity = maxCapacity;
        Completed = completed;
        Deleted = deleted;
        AssignedGuide = assignedGuide;
    }


    public static void InitializeTours()
    {
        try
        {
            LoadTours();
        }
        catch (Exception DirectoryNotFoundException)
        {
            CreateToursForToday();
        }
    }

    private static void CreateToursForToday()
    {
        dynamic settings = JsonConvert.DeserializeObject<dynamic>(Program.World.ReadAllText(Tour.JsonTourSettingsPath));
        List<dynamic> guideAssignments = JsonConvert.DeserializeObject<List<dynamic>>(Program.World.ReadAllText(Tour.JsonGuideAssignmentsPath));

        DateTime startTime = Program.World.Now.Add(TimeSpan.Parse((string)settings.StartTime));
        DateTime endTime = Program.World.Now.Add(TimeSpan.Parse((string)settings.EndTime));
        int duration = (int)settings.Duration;
        int maxCapacity = (int)settings.MaxCapacity;
        int tourInterval = 20; // Interval between tour start times

        while (startTime < endTime)
        {
            // Iterate through each guide assignment entry
            foreach (var guideEntry in guideAssignments)
            {
                var guideName = (string)guideEntry.GuideName;
                var guide = Guide.AllGuides.FirstOrDefault(g => g.Name == guideName);
                if (guide != null)
                {
                    // Assign a tour to this guide at the current start time
                    var tourId = Guid.NewGuid();
                    guide.AssignTour(tourId);  // Assign tour to guide

                    Tour newTour = new Tour(tourId, startTime, duration, maxCapacity, false, false, guide);
                    Tour.TodaysTours.Add(newTour);

                    // Increment the start time for the next tour
                    startTime = startTime.AddMinutes(tourInterval);

                    // Check if the next start time is within the end time
                    if (startTime >= endTime)
                    {
                        break;
                    }
                }
            }

            // Reset start time if it exceeds end time in the inner loop
            if (startTime >= endTime)
            {
                break;
            }
        }

        Tour.SaveTours();
    }



    public static void ShowAvailableTours()
    {
        var availableTours = TodaysTours
            .Where(tour => !tour.Completed && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > DateTime.Now)
            .OrderBy(tour => tour.StartTime)
            .ToList();

        if (availableTours.Any())
        {
            for (int i = 0; i < availableTours.Count; i++)
            {
                string formattedStartTime = availableTours[i].StartTime.ToString("h:mm tt");
                Program.World.WriteLine($"{i + 1} | Start Time: {formattedStartTime} | Duration: {availableTours[i].Duration} minutes | Remaining Spots: {availableTours[i].MaxCapacity - availableTours[i].ExpectedVisitors.Count}");
            }
        }
        else
        {
            Program.World.WriteLine("No available tours at the moment.");
        }
    }





    public void AddVisitor(Visitor visitor)
    {
        if (!Deleted && !Completed && ExpectedVisitors.Count < MaxCapacity && !ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode))
        {
            ExpectedVisitors.Add(visitor);
            SaveTours();
        }
    }

    public void RemoveVisitor(Visitor visitor)
    {
        if (ExpectedVisitors.Remove(visitor))
        {
            SaveTours();
        }
    }

    public void TransferVisitor(Visitor visitor, Tour targetTour)
    {
        if (visitor != null && !Deleted && !Completed && targetTour != null && !targetTour.Deleted && !targetTour.Completed)
        {
            RemoveVisitor(visitor);
            targetTour.AddVisitor(visitor);
        }
    }



    public static Tour FindTourByVisitorTicketCode(string ticketCode)
    {
        // Iterate through all today's tours
        foreach (var tour in TodaysTours)
        {
            // Check if any visitor in the ExpectedVisitors list has the matching ticket code
            if (tour.ExpectedVisitors.Any(visitor => visitor.TicketCode == ticketCode))
            {
                return tour;
            }
        }
        return null; // Return null if no tour with a visitor having the specified ticket code is found
    }

    public static Tour FindTourById(Guid id)
    {
        return TodaysTours.FirstOrDefault(tour => tour.TourId == id);
    }

    public static void SaveTours()
    {
        Program.World.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(TodaysTours, Formatting.Indented));
    }

    private static void LoadTours()
    {
        TodaysTours = JsonConvert.DeserializeObject<List<Tour>>(Program.World.ReadAllText(JsonFilePath));
    }

}