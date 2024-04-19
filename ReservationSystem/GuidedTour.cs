using System.Runtime.CompilerServices;
using Newtonsoft.Json;

public class GuidedTour
{
    // Non-static class
    public int Duration { get; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    //public TimeSpan TourInterval { get; private set; } // To be removed
    public int MaxCapacity { get; private set; }
    public List<Visitor> ExpectedVisitors { get; set; } = new List<Visitor>();
    public List<Visitor> PresentVisitors { get; set; } = new List<Visitor>();
    // Has to be completely public or JsonConvert.DeserializeObject refuses to set it properly
    public bool Completed { get; set; }
    // Has to be completely public or JsonConvert.DeserializeObject refuses to set it properly
    public bool Deleted { get; set; }

    //public Dictionary<DateTime, List<Visitor>> TourSlots { get; private set; } // To be removed
    public Guid TourId { get; set; }
    public Guide AssignedGuide { get; set; }

    public GuidedTour(DateTime startTime)
    {
        StartTime = startTime;
        Duration = 20; // 20 minutes
        EndTime = startTime.AddMinutes(Duration);
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

    public void AddVisitor(Visitor visitor)
    {

        // **AddVisitor** voor een gewoone **(niet een gids)** 
        // bezoeker maakt een variabel **newTour **= **this.Clone()**
        var newTour = this.Clone();
        // Check if it is a guide
        if (visitor is Guide guide)
        {
            newTour.AssignedGuide = guide;
            newTour.AssignedGuide.AssingedTourId = this.TourId;

            this.AssignedGuide = guide;
            this.AssignedGuide.AssingedTourId = this.TourId;
        }
        else
        {
            //  foundvisitor standard on false, when found will be set to true
            bool foundVisitor = false;
            foreach (Visitor currentVisitor in ExpectedVisitors)
            {
                if (visitor.TicketCode == currentVisitor.TicketCode)
                {
                    foundVisitor = true;
                    break;
                }
            }
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker moet checken dat visitor staat niet in ExpectedVisitors, if true return;
            if (foundVisitor)
            {
                return;
            }
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker moet checken of de rondleiding nog niet vol is, if true return;
            int currentCapacity = ExpectedVisitors.Count;
            if (currentCapacity == MaxCapacity)
            {
                return;
            }
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker moet checken dat property Deleted == false, if Deleted == true return;
            if (Deleted == true)
            {
                return;
            }
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker moet checken dat property **Completed **== false, if **Complete **== true return;
            if (Completed == true)
            {
                return;
            }
            
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker voegt **visitor **aan **newTour.ExpectedVisitor**
            visitor.AssingedTourId = this.TourId;
            newTour.ExpectedVisitors.Add(visitor);
            this.ExpectedVisitors.Add(visitor);
            // **AddVisitor** voor een gewoone **(niet een gids)** 
            // bezoeker gebruikts **EditTourInJSON** (this, newTour)
        }
        GuidedTour.EditTourInJSON(this, newTour);
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
        for(int visitorIndex = 0; visitorIndex<newTour.ExpectedVisitors.Count; visitorIndex++)
        {
            Visitor currentVisitor = newTour.ExpectedVisitors[visitorIndex];
            if (currentVisitor.TicketCode == visitor.TicketCode)
            {
                newTour.ExpectedVisitors.Remove(currentVisitor);
            }
        }

        // Update the tour
        GuidedTour.EditTourInJSON(this, newTour);
    }

    public void TransferVisitor(Visitor visitor, GuidedTour newTour)
    {
        if (visitor is Guide guide)
        {
            AssignedGuide = guide;
            newTour.AssignedGuide = (Guide) visitor;
            //More logic might follow deppends if we leave the guid tour inheritance from visitor
        }
        RemoveVisitor(visitor);
        AddVisitor(visitor);

    }

    public GuidedTour Clone()
    {
        var jsonString = JsonConvert.SerializeObject(this);

        return JsonConvert.DeserializeObject<GuidedTour>(jsonString);
    }

    // Static class

    public static string tourJSONpath = "./JSON-Files/GuidedTours.json";
    private Visitor visitor;
    private List<Visitor> visitors;

    public static List<DateOnly> Holidays
    {
        get;
    }
    public static List<GuidedTour> CurrentTours
    {
        get;
        private set;
    }

    public static List<GuidedTour> DeletedTours
    {
        get;
        private set;
    }

    public static List<GuidedTour> CompletedTours
    {
        get;
        private set;
    }

    static GuidedTour()
    {
        Holidays = returnHolidays(DateTime.Today.Year);
        CurrentTours = new() { };
        GuidedTour._updateCurrentTours();
    }

    public GuidedTour(DateTime startTime, Visitor visitor, List<Visitor> visitors) : this(startTime)
    {
        this.visitor = visitor;
        this.visitors = visitors;
    }

    // 
    public static GuidedTour FindTourById(Guid id)
    {
        GuidedTour foundTour = null;
        if (id == null)
            return null;

        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            if (currentTour.TourId == id)
            {
                foundTour = currentTour;
                break;
            }
        }
        foreach (GuidedTour currentTour in GuidedTour.CompletedTours)
        {
            if (currentTour.TourId == id)
            {
                foundTour = currentTour;
                break;
            }
        }
        foreach (GuidedTour currentTour in GuidedTour.DeletedTours)
        {
            if (currentTour.TourId == id)
            {
                foundTour = currentTour;
                break;
            }
        }

        if (foundTour == null)
        {
            return null;
        }

        return foundTour;
    }

    // Adds the given tour to the Json file :
    //  - Checks if the tour has correct format
    //  - Checks if the tour is in the future or in the past
    //  - Checks if the tour is already in the file, based on the _tourId
    //  - Adds the given tour to the list of tours in the static class
    //  - Updates the Json file with the list of tours in the static class
    public static void AddTourToJSON(GuidedTour tour, bool ignoreDateChecks = false)
    {
        GuidedTour._updateCurrentTours();
        TimeOnly tourTime = TimeOnly.FromDateTime(tour.StartTime);
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        bool tourAlreadyInFile = _checkIfInFile(tour);
        bool forbiddenId = CheckTourId(tour.TourId);
        if (ignoreDateChecks == false)
        {
            bool allowedTime = _checkIfAllowedTime(tourTime);
            bool allowedDate = _checkIfAllowedDate(tourDate);
            if (!allowedTime || !allowedDate || tourAlreadyInFile || forbiddenId)
            {
                return;
            }
        }

        bool tourIsInThePast = DateTime.Compare(DateTime.Now, tour.StartTime) == 1;
        bool tourIsInTheFuture = DateTime.Compare(DateTime.Now, tour.StartTime) == -1;
        bool tourIsHappeningRightNow = DateTime.Compare(DateTime.Now, tour.StartTime) == 0;
        if (tourIsInTheFuture)
        {
            GuidedTour.CurrentTours.Add(tour);
        }
        else if (tourIsInThePast)
        {
            tour.Completed = true;
            GuidedTour.CompletedTours.Add(tour);
        }
        else if (tourIsHappeningRightNow)
        {
        }


        using (StreamWriter writer = new StreamWriter(GuidedTour.tourJSONpath))
        {
            List<GuidedTour> ListOfAllTourTypes = CompletedTours.Concat(CurrentTours).ToList().Concat(DeletedTours).ToList();
            string List2json = JsonConvert.SerializeObject(ListOfAllTourTypes, Formatting.Indented);
            writer.Write(List2json);

        }

        GuidedTour._updateCurrentTours();
    }

    // Soft deletes the given tour from json file
    //  - Checks if the the given tour is actually in the json file
    //  - Checks if the the given tour is in the future or in the past
    //  - Soft delete the given tour from the list of tours in the static class
    //  - Updates the Json file with the list of tours in the static class
    public static void DeleteTourFromJSON(GuidedTour tour)
    {
        GuidedTour._updateCurrentTours();
        bool foundTour = false;
        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            if (currentTour.TourId == tour.TourId)
            {
                foundTour = true;
                break;
            }
        }
        foreach (GuidedTour currentTour in GuidedTour.CompletedTours)
        {
            if (currentTour.TourId == tour.TourId)
            {
                foundTour = true;
                break;
            }
        }

        if (foundTour == false)
        {
            return;
        }

        bool tourIsInThePast = DateTime.Compare(DateTime.Now, tour.StartTime) == 1;
        bool tourIsInTheFuture = DateTime.Compare(DateTime.Now, tour.StartTime) == -1;
        if (tourIsInTheFuture)
        {
            for (int tourIndex = 0; tourIndex < GuidedTour.CurrentTours.Count; tourIndex++)
            {
                GuidedTour currentTour = GuidedTour.CurrentTours[tourIndex];
                if (currentTour.TourId == tour.TourId)
                {
                    currentTour.Deleted = true;
                    GuidedTour.CurrentTours.Remove(currentTour);
                    GuidedTour.DeletedTours.Add(currentTour);
                    break;
                }
            }
        }
        else if (tourIsInThePast)
        {
            for (int tourIndex = 0; tourIndex < GuidedTour.CompletedTours.Count; tourIndex++)
            {
                GuidedTour currentTour = GuidedTour.CompletedTours[tourIndex];
                if (currentTour.TourId == tour.TourId)
                {
                    currentTour.Deleted = true;
                    GuidedTour.CompletedTours.Remove(currentTour);
                    GuidedTour.DeletedTours.Add(currentTour);
                    break;
                }
            }
        }

        using (StreamWriter writer = new StreamWriter(GuidedTour.tourJSONpath))
        {
            List<GuidedTour> ListOfAllTourTypes = CompletedTours.Concat(CurrentTours).ToList().Concat(DeletedTours).ToList();
            string List2json = JsonConvert.SerializeObject(ListOfAllTourTypes, Formatting.Indented);
            writer.Write(List2json);
        }
    }

    // Hard deletes the given tour from json file
    //  - Checks if the the given tour is actually in the json file
    //  - Checks if the the given tour is in the future or in the past
    //  - Hard deletes the given tour from the list of tours in the static class
    //  - Updates the Json file with the list of tours in the static class
    public static void RemoveTourFromJSON(GuidedTour tour)
    {
        GuidedTour._updateCurrentTours();
        bool foundTour = false;
        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            if (currentTour.TourId == tour.TourId)
            {
                foundTour = true;
                break;
            }
        }
        foreach (GuidedTour currentTour in GuidedTour.CompletedTours)
        {
            if (currentTour.TourId == tour.TourId)
            {
                foundTour = true;
                break;
            }
        }
        foreach (GuidedTour currentTour in GuidedTour.DeletedTours)
        {
            if (currentTour.TourId == tour.TourId)
            {
                foundTour = true;
                break;
            }
        }

        if (foundTour == false)
        {
            return;
        }

        bool tourIsInThePast = DateTime.Compare(DateTime.Now, tour.StartTime) == 1;
        bool tourIsInTheFuture = DateTime.Compare(DateTime.Now, tour.StartTime) == -1;
        if (tourIsInTheFuture)
        {
            for (int tourIndex = 0; tourIndex < GuidedTour.CurrentTours.Count; tourIndex++)
            {
                GuidedTour currentTour = GuidedTour.CurrentTours[tourIndex];
                if (currentTour.TourId == tour.TourId)
                {
                    CurrentTours.Remove(currentTour);
                }
            }
        }
        else if (tourIsInThePast)
        {
            for (int tourIndex = 0; tourIndex < GuidedTour.CompletedTours.Count; tourIndex++)
            {
                GuidedTour currentTour = GuidedTour.CompletedTours[tourIndex];
                if (currentTour.TourId == tour.TourId)
                {
                    CompletedTours.Remove(currentTour);
                }
            }
        }

        using (StreamWriter writer = new StreamWriter(GuidedTour.tourJSONpath))
        {
            List<GuidedTour> ListOfAllTourTypes = CompletedTours.Concat(CurrentTours).ToList().Concat(DeletedTours).ToList();
            string List2json = JsonConvert.SerializeObject(ListOfAllTourTypes, Formatting.Indented);
            writer.Write(List2json);
        }
    }

    // Replaces the oldTour parameter in the json file with the newTour parameter
    //  - Checks if the oldTour is in the json File
    //  - Checks if the newTour and oldTour have the same _tourId
    //  - Uses the static method DeleteTourFromJson with oldTour
    //  - Uses the static method AddTourToJSON with newTour
    public static void EditTourInJSON(GuidedTour oldTour, GuidedTour newTour, bool ignoreDateChecks = false)
    {
        GuidedTour._updateCurrentTours();
        bool oldTourExsists = GuidedTour.CurrentTours.Contains(oldTour);
        bool bothToursSameId = oldTour.TourId == newTour.TourId;

        if (oldTourExsists || !bothToursSameId)
        {
            return;
        }

        GuidedTour.RemoveTourFromJSON(oldTour);
        GuidedTour.AddTourToJSON(newTour, ignoreDateChecks);
    }

    // Updates the list of tours in the static class, based on the json file, 
    // this prevents the changes made by anything else than the app from being 
    // unnoticed by the app
    //  - Reads the GuidedTours.json file and sets variable tours to its deserialized contents
    //  - Goes through content of tours and based on properties Deleted and Completed properties add the tour to:
    //     - CurrentTours (if tour.Completed == false and tour.Deleted == false)
    //     - CompletedTours (if tour.Completed == true and tour.Deleted == false)
    //     - DeletedTours (tour.Deleted == true)
    private static void _updateCurrentTours()
    {
        DeletedTours = new();
        CurrentTours = new();
        CompletedTours = new();
        using (StreamReader reader = new(GuidedTour.tourJSONpath))
        {
            string jsonContent = reader.ReadToEnd();
            List<GuidedTour> tours = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonContent);
            //GuidedTour.CurrentTours = tours;
            for (int tourIndex = 0; tourIndex < tours.Count; tourIndex++)
            {
                GuidedTour currentTour = tours[tourIndex];
                bool currentTourIsInThePast = DateTime.Compare(DateTime.Now, currentTour.StartTime) == 1;
                if (currentTourIsInThePast && currentTour.Completed != true)
                {
                    currentTour.Completed = true;
                }

                if (currentTour.Deleted == true)
                {
                    DeletedTours.Add(currentTour);
                    continue;
                }
                else if (currentTour.Deleted == false && currentTour.Completed == false)
                {
                    CurrentTours.Add(currentTour);
                    continue;
                }
                else if (currentTour.Deleted == false && currentTour.Completed == true)
                {
                    CompletedTours.Add(currentTour);
                    continue;
                }
            }
        }
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

    public static List<GuidedTour> ReturnAllCurrentToursFromTommorow()
    {
        List<GuidedTour> tours = new();

        for (int tourIndex = 0; tourIndex < CurrentTours.Count; tourIndex++)
        {
            GuidedTour currentTour = CurrentTours[tourIndex];
            DateOnly TommorowDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
            DateTime TommorowDateTime = new(TommorowDate.Year, TommorowDate.Month, TommorowDate.Day);

            bool sameYear = TommorowDate.Year == currentTour.StartTime.Year;
            bool sameMonth = TommorowDate.Month == currentTour.StartTime.Month;
            bool tourIsTommorow = TommorowDate.Day == currentTour.StartTime.Day && sameMonth && sameYear;
            if (tourIsTommorow)
            {
                tours.Add(currentTour);
            }
        }

        // sort list using linq
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

        List<GuidedTour> toursTommorow = GuidedTour.ReturnAllCurrentToursFromTommorow();
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

    // Returns if the given Guid is already a TourId in a different tour
    private static bool CheckTourId(Guid id)
    {
        bool foundId = false;
        List<Guid> ids = new();

        foreach (GuidedTour tour in GuidedTour.CurrentTours)
        {
            ids.Add(tour.TourId);
        }
        foreach (GuidedTour tour in GuidedTour.CompletedTours)
        {
            ids.Add(tour.TourId);
        }
        foreach (GuidedTour tour in GuidedTour.DeletedTours)
        {
            ids.Add(tour.TourId);
        }

        if (ids.Contains(id))
        {
            foundId = true;
        }

        return foundId;
    }

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

    private static bool _checkIfAllowedTime(TimeOnly time)
    {
        List<int> allowedHours = new List<int>() { 9, 10, 11, 12, 13, 14, 15, };
        bool allowed = true;

        if (time.Minute != 0 && time.Minute != 20 && time.Minute != 40)
        {
            allowed = false;
        }
        if (!allowedHours.Contains(time.Hour))
        {
            allowed = false;
        }
        if (time.Hour == 16 && (time.Minute == 20 || time.Minute == 0))
        {
            allowed = true;
        }

        return allowed;
    }

    private static bool _checkIfAllowedDate(DateOnly date)
    {
        List<DateOnly> mondays = returnEveryMondayThisYear();
        bool allowed = true;

        if (Holidays.Contains(date))
        {
            allowed = false;
        }

        if (mondays.Contains(date))
        {
            allowed = false;
        }

        return allowed;
    }

    // Checks if a given tour is already in the json, based on the TourId
    private static bool _checkIfInFile(GuidedTour tour)
    {
        GuidedTour._updateCurrentTours();
        foreach (GuidedTour currentTour in GuidedTour.CurrentTours)
        {
            bool sameId = currentTour.TourId == tour.TourId;
            //bool sameDateTime = currentTour.StartTime == tour.StartTime;
            if (/*sameDateTime ||*/ sameId)
            {
                return true;
            }
        }
        return false;
    }

    // Returns all tours from this year
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
    // makes all possible tour for a given date
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

    // Check if given visitor is in any tour
    //updated methode to use linq to be faster and more efficent. 
    public static bool CheckIfVisitorInTour(Visitor visitor)
    {
        return CurrentTours.Any(tour => tour.ExpectedVisitors.Any(v => v.VisitorId == visitor.VisitorId));
    }

    public void ChangeTime(DateTime newDate)
    {
        DateOnly newDateOnly = new DateOnly(newDate.Year, newDate.Month, newDate.Day);
        TimeOnly newTimeOnly = new TimeOnly(newDate.Hour, newDate.Minute, newDate.Second);
        if (!_checkIfAllowedDate(newDateOnly))
        {
            return;
        }
        if (!_checkIfAllowedTime(newTimeOnly))
        {
            return;
        }
    }
}

//     private bool _checkIfAllowedTime(TimeOnly time)
//     {
//         if (time.Hour >= 9 && time.Hour <= 16)
//         {
//             // if bellow 40Min on the 16th hour return true
//             if (Time.Hour == 16) { return time.Minute < 40; }
//             // if between 9th and 15th hour return true
//             else { return true;}
//         }
//         return false;
//     }
// }