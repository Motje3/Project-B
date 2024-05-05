using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public class GuidedTour
{
    public int Duration { get; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int MaxCapacity { get; private set; }
    public List<Visitor> ExpectedVisitors { get; set; } = new List<Visitor>();
    public List<Visitor> PresentVisitors { get; set; } = new List<Visitor>();
    public bool Completed { get; set; }
    public bool Deleted { get; set; }
    public Guid TourId { get; set; }
    public Guide AssignedGuide { get; set; }

    public static List<DateOnly> Holidays { get; private set; }
    public static List<GuidedTour> CurrentTours { get; private set; }
    public static List<GuidedTour> DeletedTours { get; private set; }
    public static List<GuidedTour> CompletedTours { get; private set; }

    public GuidedTour(DateTime startTime)
    {
        StartTime = startTime;
        Duration = 20;
        EndTime = StartTime.AddMinutes(Duration);
        MaxCapacity = 13;
        TourId = Guid.NewGuid();
        Completed = false;
        Deleted = false;
    }

    // constructor for json serializer DO NOT USE IT WILL PROBABLY BREAK SOMETHING
    [JsonConstructor]
    public GuidedTour(int duration, DateTime startTime, DateTime endTime, int maxCapacity, Guid tourId, bool complete, bool deleted, Guide assignedGuide)
    {
        Duration = duration;
        StartTime = startTime;
        EndTime = endTime;
        MaxCapacity = maxCapacity;
        TourId = tourId;
        Completed = complete;
        Deleted = deleted;
        AssignedGuide = assignedGuide;
    }

    public GuidedTour Clone()
    {
        var jsonString = JsonConvert.SerializeObject(this);

        return JsonConvert.DeserializeObject<GuidedTour>(jsonString);
    }

    public static string JsonFilePath = "./DataLayer/JSON-Files/GuidedTours.json";

    public void AddVisitor(Visitor visitor)
    {
        var newTour = Clone();

        if (visitor is Guide guide)
        {
            newTour.AssignedGuide = guide;
            newTour.AssignedGuide.AssingedTourId = TourId;
            AssignedGuide = guide;
            AssignedGuide.AssingedTourId = TourId;
        }
        else
        {
            // Check if visitor already exists, or if the tour cannot accept more visitors
            if (ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode) ||
                ExpectedVisitors.Count >= MaxCapacity ||
                Deleted ||
                Completed)
            {
                return; // Exit early if any condition is met that prevents adding a new visitor
            }

            // Set the tour ID for the visitor and add them to both the current and the new cloned tour lists
            visitor.AssingedTourId = TourId;
            newTour.ExpectedVisitors.Add(visitor);
            ExpectedVisitors.Add(visitor);
        }
        JsonHelper.EditTour(newTour, TourId, GuidedTour.JsonFilePath);
    }


    public void RemoveVisitor(Visitor visitor)
    {
        // Check if the visitor is a guide
        if (visitor is Guide)
        {
            // If the visitor is a guide, remove their assignment
            AssignedGuide = null;
            return;
        }

        // Check if the visitor is found
        bool foundVisitor = false;
        foreach (Visitor currentVisitor in ExpectedVisitors)
        {
            if (visitor.TicketCode == currentVisitor.TicketCode)
            {
                foundVisitor = true;
                break;
            }
        }
        if (!foundVisitor)
        {
            return;
        }
        // Create a new tour to modify
        GuidedTour newTour = this.Clone();

        // Remove the visitor from ExpectedVisitors
        for (int visitorIndex = 0; visitorIndex < newTour.ExpectedVisitors.Count; visitorIndex++)
        {
            Visitor currentVisitor = newTour.ExpectedVisitors[visitorIndex];
            if (currentVisitor.TicketCode == visitor.TicketCode)
            {
                newTour.ExpectedVisitors.Remove(currentVisitor);
                break; // Ensure to break after removal to avoid InvalidOperationException
            }
        }

        // Update the tour
        JsonHelper.EditTour(newTour, TourId, GuidedTour.JsonFilePath);
    }


    public void TransferVisitor(Visitor visitor, GuidedTour newTour)
    {
        if (visitor == null)
        { return; }

        RemoveVisitor(visitor);
        ExpectedVisitors.Remove(visitor);
        newTour.AddVisitor(visitor);
    }

    public static void _updateCurrentTours()
    {
        var allTours = JsonHelper.LoadFromJson<List<GuidedTour>>(JsonFilePath);
        CurrentTours = new List<GuidedTour>();
        CompletedTours = new List<GuidedTour>();
        DeletedTours = new List<GuidedTour>();

        foreach (var tour in allTours)
        {
            if (tour.Deleted)
                DeletedTours.Add(tour);
            else if (tour.Completed)
                CompletedTours.Add(tour);
            else
                CurrentTours.Add(tour);
        }
    }

    //Needs to be adjusted to allow only changes to upcoming tours and never tours in the same day, so no need to check for expected visitors
    public void ChangeCapacity(int newCapacity)
    {
        // Cloning the current state of the tour to modify
        GuidedTour newTour = Clone();
        int currentMinimumCapacity = ExpectedVisitors.Count;

        // Check and set the new capacity based on the number of expected visitors
        if (newCapacity < currentMinimumCapacity)
        {
            newTour.MaxCapacity = currentMinimumCapacity; // Set to the minimum needed to accommodate all expected visitors
        }
        else
        {
            newTour.MaxCapacity = newCapacity; // Set to the new desired capacity
        }

        // Ensuring the current tour's maximum capacity is also updated
        MaxCapacity = newTour.MaxCapacity;

        // Assuming GuidedTour.JsonFilePath holds the path to the JSON file for tours
        if (GuidedTour.JsonFilePath == null || TourId == Guid.Empty)
        {
            Console.WriteLine("Invalid file path or tour ID.");
            return;
        }

        // Update the tour in the JSON file using JsonHelper
        JsonHelper.EditTour(newTour, TourId, GuidedTour.JsonFilePath);
        Console.WriteLine($"Tour capacity changed to {newTour.MaxCapacity}.");
    }




    private Visitor visitor;
    private List<Visitor> visitors;
    static GuidedTour()
    {
        Holidays = returnHolidays(DateTime.Today.Year);
        CurrentTours = new() { };
        _updateCurrentTours();
    }

    public GuidedTour(DateTime startTime, Visitor visitor, List<Visitor> visitors) : this(startTime)
    {
        this.visitor = visitor;
        this.visitors = visitors;
    }



    //Can be written alot shorter
    public static GuidedTour FindTourById(Guid id)
    {
        // Concatenate all lists of tours into a single sequence for searching
        var allTours = CurrentTours.Concat(GuidedTour.CompletedTours).Concat(GuidedTour.DeletedTours);

        // Use LINQ to find the first tour matching the given ID
        return allTours.FirstOrDefault(tour => tour.TourId == id);
    }

    // Returns all tours from today that have not yet taken place
    //  - checks which tours have already started
    //  - checks which tours are today
    //  - at the end sorts starting from the earliest tour
    public static List<GuidedTour> ReturnAllCurrentToursFromToday()
    {
        List<GuidedTour> tours = new();

        for (int tourIndex = 0; tourIndex < CurrentTours.Count; tourIndex++)
        {
            GuidedTour currentTour = CurrentTours[tourIndex];
            DateOnly TommorowDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            DateTime TommorowDateTime = new(TommorowDate.Year, TommorowDate.Month, TommorowDate.Day);

            bool tourNotYetStarted = DateTime.Compare(DateTime.Now, currentTour.StartTime) == -1;
            bool tourIsToday = DateTime.Compare(currentTour.StartTime, TommorowDateTime) == -1;

            if (tourNotYetStarted && tourIsToday)
            {
                tours.Add(currentTour);
            }
        }

        // sort list using linq
        tours = tours.OrderBy(tour => tour.StartTime).ToList();

        return tours;
    }

    public static List<GuidedTour> ReturnToursFromNextDay()
    {
        List<GuidedTour> tours = new();
        DateOnly TodayDate = DateOnly.FromDateTime(DateTime.Today);
        DateOnly NextDayDate = new(1, 1, 1);

        for (int tourIndex = 0; tourIndex < CurrentTours.Count; tourIndex++)
        {
            GuidedTour currentTour = CurrentTours[tourIndex];
            DateOnly currTourDate = DateOnly.FromDateTime(currentTour.StartTime);
            bool tourIsToday = currTourDate == TodayDate;
            if (tourIsToday || currTourDate.DayOfWeek == DayOfWeek.Monday)
            {
                continue;
            }
            else if (NextDayDate.Year == 1)
            {
                NextDayDate = currTourDate;
                tours.Add(currentTour);
            }
            else
            {
                if (currTourDate == NextDayDate)
                {
                    tours.Add(currentTour);
                }
            }
        }

        tours = tours.OrderBy(tour => tour.StartTime).ToList();
        return tours;
    }

    // Prints all tours from today that are not full yet
    //  - Also returns a list of tours that are not full yet
    //  - Also prints tours from tommorw until 10 allowedTours if less than 10 are present
    public static List<GuidedTour> PrintToursOpenToday()
    {
        // Print all avaible tours could be simplified in a methode that is in guidedtour. 
        List<GuidedTour> allowedTours = new();
        List<GuidedTour> todayTours = GuidedTour.ReturnAllCurrentToursFromToday();
        int allowedTourIndex = 0;
        for (int tourIndex = 0; tourIndex < todayTours.Count; tourIndex++)
        {
            GuidedTour currentTour = todayTours[tourIndex];

            int spacesLeftInTour = currentTour.MaxCapacity - currentTour.ExpectedVisitors.Count;
            if (spacesLeftInTour >= 0)
            {
                allowedTours.Add(currentTour);

                DateOnly tourDate = DateOnly.FromDateTime(currentTour.StartTime);
                string hour = currentTour.StartTime.Hour.ToString();
                string minute = currentTour.StartTime.Minute.ToString();
                if (minute == "0")
                    minute = "00";
                Console.WriteLine($"{allowedTourIndex + 1} | {hour}:{minute} {tourDate} | duration: {currentTour.Duration} minutes | {spacesLeftInTour} places remaining ");
                allowedTourIndex++;
            }
        }

        List<GuidedTour> toursTommorow = GuidedTour.ReturnToursFromNextDay();
        int tommorowTourIndex = 0;
        // if they are less than 10 allowedTours present, add tours from tommorow until 10 
        while (allowedTours.Count < 10)
        {
            GuidedTour currentTour = toursTommorow[tommorowTourIndex];

            int spacesLeftInTour = currentTour.MaxCapacity - currentTour.ExpectedVisitors.Count;
            if (spacesLeftInTour >= 0)
            {
                allowedTours.Add(currentTour);

                DateOnly tourDate = DateOnly.FromDateTime(currentTour.StartTime);
                string hour = currentTour.StartTime.Hour.ToString();
                string minute = currentTour.StartTime.Minute.ToString();
                if (minute == "0")
                    minute = "00";
                Console.WriteLine($"{allowedTourIndex + 1} | {hour}:{minute} {tourDate} | duration: {currentTour.Duration} minutes | {spacesLeftInTour} places remaining ");
                allowedTourIndex++;
            }

            tommorowTourIndex++;
        }

        return allowedTours;
    }

    //Not needed as Guid can never be the same, the chances of aliens bringing us a gift tomorrow is higher
    private static bool CheckTourId(Guid id)
    {
        // Combine all lists and check if any tour has the specified ID.
        return CurrentTours.Concat(CompletedTours).Concat(DeletedTours).Any(tour => tour.TourId == id);
    }


    //ig we might need it later
    private static List<DateOnly> returnHolidays(int year)
    {
        DateOnly Nieuwjaarsdag = new(year, 1, 1); // Nieuwjaarsdag: maandag 1 januari 2024
        DateOnly GoedeVrijdag = new(year, 3, 29); // Goede Vrijdag: vrijdag 29 maart 2024
        DateOnly DagTussen = new(year, 3, 30); // Pasen (eerste en tweede paasdag): zondag 31 maart en maandag 1 april 2024
        DateOnly Pasen1 = new(year, 3, 31); // Pasen (eerste en tweede paasdag): zondag 31 maart en maandag 1 april 2024
        DateOnly Pasen2 = new(year, 4, 1); // Pasen (eerste en tweede paasdag): zondag 31 maart en maandag 1 april 2024
        DateOnly Koningsdag = new(year, 4, 27); // Koningsdag: zaterdag 27 april 2024
        DateOnly Bevrijdingsdag = new(year, 5, 5); // Bevrijdingsdag: zondag 5 mei 2024
        DateOnly Hemelvaartsdag = new(year, 5, 9); // Hemelvaartsdag: donderdag 9 mei 2024
        DateOnly Pinksteren1 = new(year, 5, 19); // Pinksteren (eerste en tweede pinksterdag): zondag 19 en maandag 20 mei 2024
        DateOnly Pinksteren2 = new(year, 5, 20); // Pinksteren (eerste en tweede pinksterdag): zondag 19 en maandag 20 mei 2024
        DateOnly Kerstmis1 = new(year, 12, 25); // Kerstmis (eerste en tweede kerstdag): woensdag 25 en donderdag 26 december 2024
        DateOnly Kerstmis2 = new(year, 12, 26); // Kerstmis (eerste en tweede kerstdag): woensdag 25 en donderdag 26 december 2024

        return new() { Nieuwjaarsdag, GoedeVrijdag, DagTussen, Pasen1, Pasen2, Koningsdag, Bevrijdingsdag, Hemelvaartsdag, Pinksteren1, Pinksteren2, Kerstmis1, Kerstmis2 };
    }

    //no idea why
    private static List<DateOnly> returnEveryMondayThisYear()
    {
        List<DateOnly> mondays = new();
        for (int dayIndex = 0; dayIndex < 365; dayIndex++)
        {
            DateOnly day = new(DateTime.Today.Year, 1, 1);
            day = day.AddDays(dayIndex - 1);

            if (day.DayOfWeek == DayOfWeek.Monday)
            {
                mondays.Add(day);
            }
        }
        return mondays;
    }


    // Checks if a given tour is already in the json, based on the TourID. (Can be written in one line)
    private static bool _checkIfInFile(GuidedTour tour)
    {
        _updateCurrentTours();
        return CurrentTours.Any(currentTour => currentTour.TourId == tour.TourId);
    }


    // Returns all tours from this year (Why? )
    public static List<GuidedTour> ReturnAllToursFromThisYear()
    {
        int thisYear = DateTime.Today.Year;
        DateOnly yearStart = new(thisYear, 1, 1);
        List<GuidedTour> tours = new();

        for (int dayIndex = 0; dayIndex < 365; dayIndex++)
        {
            DateOnly day = yearStart.AddDays(dayIndex);
            List<GuidedTour> toursToday = _makeToursForDay(day);
            foreach (GuidedTour tour in toursToday)
            {
                tours.Add(tour);
            }
        }
        return tours;
    }

    // makes all possible tour for a given date (might be needed for admin to make tours)
    private static List<GuidedTour> _makeToursForDay(DateOnly date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;
        List<GuidedTour> tours = new();
        List<int> hours = new() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        for (int hourIndex = 0; hourIndex < hours.Count; hourIndex++)
        {
            int hour = hours[hourIndex];
            GuidedTour Tour1 = new GuidedTour(new(year, month, day, hour, 0, 0));
            GuidedTour Tour2 = new GuidedTour(new(year, month, day, hour, 20, 0));
            GuidedTour Tour3 = new GuidedTour(new(year, month, day, hour, 40, 0));
            tours.Add(Tour1);
            tours.Add(Tour2);
            tours.Add(Tour3);
        }

        return tours;
    }

    public void ChangeTime(DateTime newDate)
    {
        GuidedTour newTour = Clone();
        newTour.StartTime = newDate;
        newTour.EndTime = newTour.StartTime.AddMinutes(newTour.Duration);
        JsonHelper.EditTour(newTour, TourId, JsonFilePath);
    }
}
