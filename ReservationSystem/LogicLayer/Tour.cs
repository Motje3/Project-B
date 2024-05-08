using Newtonsoft.Json;

public class Tour
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
    public static List<Tour> CurrentTours { get; private set; }
    public static List<Tour> DeletedTours { get; private set; }
    public static List<Tour> CompletedTours { get; private set; }

    public Tour(DateTime startTime)
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
    public Tour(int duration, DateTime startTime, DateTime endTime, int maxCapacity, Guid tourId, bool complete, bool deleted, Guide assignedGuide)
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

    public Tour Clone()
    {
        var jsonString = JsonConvert.SerializeObject(this);

        return JsonConvert.DeserializeObject<Tour>(jsonString);
    }

    public static string JsonFilePath = "./DataLayer/JSON-Files/GuidedTours.json";

    public void AddVisitor(Visitor visitor)
    {
        var newTour = Clone();

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

        JsonHelper.EditTour(newTour, TourId, JsonFilePath);
    }



    public void RemoveVisitor(Visitor visitor)
    {

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
        Tour newTour = Clone();

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

        JsonHelper.EditTour(newTour, TourId, JsonFilePath);
    }


    public void TransferVisitor(Visitor visitor, Tour newTour)
    {
        if (visitor == null)
        { return; }

        RemoveVisitor(visitor);
        ExpectedVisitors.Remove(visitor);
        newTour.AddVisitor(visitor);
    }

    public static void _updateCurrentTours()
    {
        var allTours = JsonHelper.LoadFromJson<List<Tour>>(JsonFilePath);
        CurrentTours = new List<Tour>();
        CompletedTours = new List<Tour>();
        DeletedTours = new List<Tour>();

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
        Tour newTour = Clone();
        int currMinCapacity = ExpectedVisitors.Count;

        if (newCapacity < currMinCapacity)
        {
            newTour.MaxCapacity = currMinCapacity;
            MaxCapacity = currMinCapacity;
        }
        else
        {
            newTour.MaxCapacity = newCapacity;
            MaxCapacity = newCapacity;
        }

        JsonHelper.EditTour(this, newTour.TourId, JsonFilePath);
    }

    public void ChangeTime(DateTime newDate)
    {
        Tour newTour = Clone();
        newTour.StartTime = newDate;
        newTour.EndTime = newTour.StartTime.AddMinutes(newTour.Duration);
        JsonHelper.EditTour(newTour, TourId, JsonFilePath);
    }


    static Tour()
    {
        CurrentTours = new() { };
        _updateCurrentTours();
    }



    public static Tour FindTourById(Guid id)
    {
        var allTours = CurrentTours.Concat(CompletedTours).Concat(DeletedTours);

        return allTours.FirstOrDefault(tour => tour.TourId == id);
    }

    // Returns all tours from today that have not yet taken place
    //  - checks which tours have already started
    //  - checks which tours are today
    //  - at the end sorts starting from the earliest tour
    public static List<Tour> ReturnAllCurrentToursFromToday()
    {
        List<Tour> tours = new();

        for (int tourIndex = 0; tourIndex < CurrentTours.Count; tourIndex++)
        {
            Tour currentTour = CurrentTours[tourIndex];
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

    public static List<Tour> ReturnToursFromNextDay()
    {
        List<Tour> tours = new();
        DateOnly TodayDate = DateOnly.FromDateTime(DateTime.Today);
        DateOnly NextDayDate = new(1, 1, 1);

        for (int tourIndex = 0; tourIndex < CurrentTours.Count; tourIndex++)
        {
            Tour currentTour = CurrentTours[tourIndex];
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
    public static List<Tour> PrintToursOpenToday()
    {
        // Print all avaible tours could be simplified in a methode that is in guidedtour. 
        List<Tour> allowedTours = new();
        List<Tour> todayTours = Tour.ReturnAllCurrentToursFromToday();
        int allowedTourIndex = 0;
        for (int tourIndex = 0; tourIndex < todayTours.Count; tourIndex++)
        {
            Tour currentTour = todayTours[tourIndex];

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

        List<Tour> toursTommorow =   ReturnToursFromNextDay();
        int tommorowTourIndex = 0;
        // if they are less than 10 allowedTours present, add tours from tommorow until 10 
        while (allowedTours.Count < 10)
        {
            Tour currentTour = toursTommorow[tommorowTourIndex];

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
    private static bool ReturnTourFromID(Guid id)
    {
        // Combine all lists and check if any tour has the specified ID.
        return CurrentTours.Concat(CompletedTours).Concat(DeletedTours).Any(tour => tour.TourId == id);
    }


}
