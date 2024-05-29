using ReservationSystem;
using Newtonsoft.Json;

public class TourDataManager
{
    public static void SaveTours()
    {
        Program.World.WriteAllText(TourTools.JsonFilePath, JsonConvert.SerializeObject(TourTools.TodaysTours, Formatting.Indented));
    }

    public static void LoadTours()
    {
        TourTools.TodaysTours = JsonConvert.DeserializeObject<List<Tour>>(Program.World.ReadAllText(TourTools.JsonFilePath));

        foreach (var tour in TourTools.TodaysTours)
        {
            if (tour.AssignedGuide != null)
            {
                var guide = Guide.AllGuides.FirstOrDefault(g => g.GuideId == tour.AssignedGuide.GuideId);
                if (guide != null)
                {
                    guide.AssignTour(tour.TourId);
                    tour.AssignedGuide = guide; // Ensure the reference is updated correctly
                }
            }
        }
    }

    public static void CreateToursForToday()
    {
        dynamic settings = JsonConvert.DeserializeObject<dynamic>(Program.World.ReadAllText(TourTools.JsonTourSettingsPath));
        List<dynamic> guideAssignments = JsonConvert.DeserializeObject<List<dynamic>>(Program.World.ReadAllText(TourTools.JsonGuideAssignmentsPath));

        DateTime startTime = Program.World.Today.Add(TimeSpan.Parse((string)settings.StartTime));
        DateTime endTime = Program.World.Today.Add(TimeSpan.Parse((string)settings.EndTime));
        int duration = (int)settings.Duration; // This remains 40 minutes
        int maxCapacity = (int)settings.MaxCapacity;

        foreach (var guideEntry in guideAssignments)
        {
            var guideName = (string)guideEntry.GuideName;
            var guide = Guide.AllGuides.FirstOrDefault(g => g.Name == guideName);
            if (guide != null)
            {
                foreach (var tourEntry in guideEntry.Tours)
                {
                    var tourStartTime = Program.World.Today.Add(DateTime.Parse((string)tourEntry.StartTime).TimeOfDay);
                    if (tourStartTime >= startTime && tourStartTime < endTime)
                    {
                        var tourId = Guid.NewGuid();
                        guide.AssignTour(tourId);  // Assign tour to guide

                        Tour newTour = new Tour(tourId, tourStartTime, duration, maxCapacity, false, false, guide);
                        TourTools.TodaysTours.Add(newTour);
                    }
                }
            }
        }
        TourTools.TodaysTours.Sort((x, y) => DateTime.Compare(x.StartTime, y.StartTime));

        SaveTours();
    }

    // FilterByLamda is used filter on specific requirement, 
    // example ussage: var toursByAlice = TourFilter.FilterByLambda(t => t.AssignedGuide.Name == "Alice Johnson");
    public static List<Tour> FilterByLambda(Func<Tour, bool> filter)  // this can be used for multiple purpeses where you need to sort it for specific required data
    {
        TourTools.TodaysTours = JsonConvert.DeserializeObject<List<Tour>>(File.ReadAllText(TourTools.JsonFilePath));
        var FilterdTours = TourTools.TodaysTours.Where(filter).ToList();
        return FilterdTours;
    }
}