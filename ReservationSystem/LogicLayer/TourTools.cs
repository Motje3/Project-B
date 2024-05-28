using ReservationSystem;

public class TourTools
{
    public static List<Tour> TodaysTours { get; set; } = new List<Tour>();
    public static string JsonFilePath => $"./JSON-Files/Tours-{Program.World.Today:yyyyMMdd}.json";
    public static string JsonTourSettingsPath => $"./JSON-Files/TourSettings.json";
    public static string JsonGuideAssignmentsPath => $"./JSON-Files/GuideAssignments.json";

    public static void InitializeTours()
    {
        try
        {
            TourDataManager.LoadTours();
        }
        catch(FileNotFoundException)
        {
            TourDataManager.CreateToursForToday();
        }
    }

    public static bool ShowAvailableTours()
    {
        var availableTours = TodaysTours
            .Where(tour => !tour.Started && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity && tour.StartTime > Program.World.Now)
            .OrderBy(tour => tour.StartTime)
            .ToList();

        if (availableTours.Any())
        {
            for (int i = 0; i < availableTours.Count; i++)
            {
                string formattedStartTime = availableTours[i].StartTime.ToString("HH:mm");
                string tourNumber = (i + 1).ToString();

                // Print the number and the mark in cyan
                ColourText.WriteColored("", tourNumber + " | ", ConsoleColor.Cyan);

                // Print the start time in cyan
                ColourText.WriteColored("Start Time: ", formattedStartTime, ConsoleColor.Yellow);

                // Print the rest of the text in default color
                Console.WriteLine($" | Duration: {availableTours[i].Duration} minutes | Remaining Spots: {availableTours[i].MaxCapacity - availableTours[i].ExpectedVisitors.Count}");

            }
            return true;
        }
        else
        {
            try { Console.Clear(); } catch { }

            Program.World.WriteLine("No available tours left today. Please try again tomorrow!");
            Thread.Sleep(2000);
            return false;
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

}