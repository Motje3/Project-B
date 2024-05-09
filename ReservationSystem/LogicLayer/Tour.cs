using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public static string JsonFilePath => $"./DataLayer/JSON-Files/Tours-{DateTime.Today:yyyyMMdd}.json";
    public static string JsonTourSettingsPath => $"./DataLayer/JSON-Files/TourSettings.json";

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
        if (File.Exists(JsonFilePath))
        {
            LoadTours();
        }
        else
        {
            CreateToursForToday();
        }
    }

    private static void CreateToursForToday()
    {
        dynamic settings = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(JsonTourSettingsPath));
        DateTime startTime = DateTime.Today.Add(TimeSpan.Parse((string)settings.StartTime));
        DateTime endTime = DateTime.Today.Add(TimeSpan.Parse((string)settings.EndTime));
        int duration = (int)settings.Duration;
        int maxCapacity = (int)settings.MaxCapacity;

        while (startTime < endTime)
        {
            TodaysTours.Add(new Tour(Guid.NewGuid(), startTime, duration, maxCapacity, false, false, null));
            startTime = startTime.AddMinutes(duration);
        }

        SaveTours();
    }

    public static void ShowAvailableTours()
    {
        var availableTours = TodaysTours.Where(tour => !tour.Completed && !tour.Deleted && tour.ExpectedVisitors.Count < tour.MaxCapacity).ToList();
        if (availableTours.Any())
        {

            for (int i = 0; i < availableTours.Count; i++)
            {
                string formattedStartTime = availableTours[i].StartTime.ToString("h:mm tt");
                Console.WriteLine($"{i + 1} | Start Time: {formattedStartTime} | Duration: {availableTours[i].Duration} minutes | Current Capacity: {availableTours[i].ExpectedVisitors.Count}/{availableTours[i].MaxCapacity}");
            }
        }
        else
        {
            Console.WriteLine("No available tours at the moment.");
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
        File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(TodaysTours, Formatting.Indented));
    }

    private static void LoadTours()
    {
        TodaysTours = JsonConvert.DeserializeObject<List<Tour>>(File.ReadAllText(JsonFilePath));
    }
}