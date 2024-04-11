using Newtonsoft.Json;
using System.Globalization;


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
    public int TourId { get; }

    public GuidedTour(DateTime startTime)
    {
        StartTime = startTime;
        Duration = 20; // 20 minutes
        EndTime = startTime.AddMinutes(Duration);
        MaxCapacity = 13;
        TourId = GuidedTour._generateUniqueId();


        //TourSlots = new Dictionary<DateTime, List<Visitor>>();
        //LoadTourSettings();
        //InitializeTourSlotsForToday(); // Now it's safe to call this
    }

    public GuidedTour(DateTime startTime, int tourId)
    {
        StartTime = startTime;
        Duration = 20; // 20 minutes
        EndTime = startTime.AddMinutes(Duration);
        MaxCapacity = 13;
        TourId = tourId;
    }

    // constructor for json serializer DO NOT USE IT WILL PROBABLY BREAK SOMETHING
    [JsonConstructor]
    public GuidedTour(int duration, DateTime startTime, DateTime endTime, int maxCapacity, int tourId, bool complete, bool deleted)
    {
        Duration = duration;
        StartTime = startTime;
        EndTime = endTime;
        MaxCapacity = maxCapacity;
        TourId = tourId;
        Completed = complete;
        Deleted = deleted;
    }

    


    /*private void LoadTourSettings()
    {
        string filePath = "./JSON-Files/TourSettings.json";

        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                dynamic settings = JsonConvert.DeserializeObject(json);

                // Parse the times as TimeSpan objects
                TimeSpan startTimeSpan = TimeSpan.Parse((string)settings?.StartTime);
                TimeSpan endTimeSpan = TimeSpan.Parse((string)settings?.EndTime);

                // Combine today's date with the loaded times
                DateTime today = DateTime.Today;
                StartTime = today.Add(startTimeSpan);
                EndTime = today.Add(endTimeSpan);

                TourInterval = TimeSpan.FromMinutes((int)(settings?.TourInterval ?? 20));
                MaxCapacity = (int)(settings?.MaxCapacity ?? 13);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tour settings: {ex.Message}");
                // Handle the error, possibly by setting default values
            }
        }
        else
        {
            Console.WriteLine("Tour settings file not found. Using default settings.");

            DateTime today = DateTime.Today;
            StartTime = today.AddHours(9); // Start at 9 AM today
            EndTime = today.AddHours(17); // End at 5 PM today
            TourInterval = TimeSpan.FromMinutes(20); // 20-minute interval between tours
            MaxCapacity = 13; // Default maximum capacity
        }
    }*/




    /*private void InitializeTourSlotsForToday()
    {
        ClearTourSlots();

        DateTime today = DateTime.Today;
        DateTime startTimeToday = new DateTime(today.Year, today.Month, today.Day, StartTime.Hour, StartTime.Minute, 0);
        DateTime endTimeToday = new DateTime(today.Year, today.Month, today.Day, EndTime.Hour, EndTime.Minute, 0);

        // Debugging output
        // Console.WriteLine($"Initializing slots from {startTimeToday} to {endTimeToday}");

        DateTime slotTime = startTimeToday;
        while (slotTime < endTimeToday)
        {
            if (!TourSlots.ContainsKey(slotTime))
            {
                TourSlots.Add(slotTime, new List<Visitor>());
                // More debugging output for each slot
            }
            slotTime = slotTime.Add(TourInterval);
        }
    }*/


    /*private void ClearTourSlots()
    {
        if (TourSlots == null)
        {
            TourSlots = new Dictionary<DateTime, List<Visitor>>();
        }
        else
        {
            TourSlots.Clear();
        }
    }



    public void ListAvailableTours(int numberOfPeopleAttemptingToJoin)
    {
        DateTime now = DateTime.Now;

        Console.WriteLine("Available tour times:");
        foreach (var slot in TourSlots)
        {
            if ((slot.Value.Count + numberOfPeopleAttemptingToJoin) <= MaxCapacity)
            {
                Console.WriteLine($"{slot.Key.ToString("MM/dd/yyyy h:mm tt")} - {slot.Value.Count} participants");
            }

        }
    }


    public bool JoinTour(DateTime tourTime, Visitor visitor)
    {

        if (tourTime < StartTime || tourTime > EndTime)
        {
            Console.WriteLine("The chosen time is outside the tour operation hours.");
            return false;
        }

        // If we get here, the tour is either full or the hour doesn't have a slot.
        if (!TourSlots.ContainsKey(tourTime))
        {
            Console.WriteLine("There is no tour at the chosen time.");
            return false;
        }

        // if (tourTime <= DateTime.Now)
        // {
        //     Console.WriteLine("Cannot join a tour that has already started or passed.");
        //     return false;
        // }

        // Check if the slot exists and is not full.
        if (TourSlots[tourTime].Count < MaxCapacity)
        {
            TourSlots[tourTime].Add(visitor);
            Console.WriteLine($"{visitor.Name} has successfully joined the tour at {tourTime.ToString("h:mm tt")}");
            return true;
        }
        else
        {
            Console.WriteLine("Could not join tour.");
            return false;
        }
    }


    public bool UpdateVisitorTour(string ticketCode, DateTime newTourDateTime)
    {
        bool anyUpdates = false; // Flag to track if any updates were made

        try
        {
            // Collect all visitors across all slots with the given ticketCode
            var visitorsToUpdate = new List<Visitor>();
            foreach (var slot in TourSlots)
            {
                visitorsToUpdate.AddRange(slot.Value.Where(v => v.TicketCode == ticketCode).ToList());
            }

            // Check if the new slot has enough capacity for all visitors
            if (TourSlots.TryGetValue(newTourDateTime, out List<Visitor> newSlot) && newSlot.Count + visitorsToUpdate.Count <= MaxCapacity)
            {
                foreach (var visitor in visitorsToUpdate)
                {
                    // Remove visitor from their current slot
                    foreach (var slot in TourSlots.Values)
                    {
                        slot.Remove(visitor);
                    }

                    // Add visitor to the new slot
                    newSlot.Add(visitor);
                    anyUpdates = true; // Mark that at least one update was made
                }
            }
            else
            {
                Console.WriteLine("The tour at the new hour is full or does not exist.");
                return false; // Indicate failure due to full capacity or non-existent slot
            }

            if (anyUpdates)
            {
                Console.WriteLine("All visitors reservation's updated successfully.");
                return true; // Successful update for all visitors
            }
            else
            {
                Console.WriteLine("No visitors found with the given ticket code.");
                return false; // No visitors were found and updated
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return false; // Indicate failure due to an exception
        }
    }



    public bool RemoveVisitorFromTour(DateTime tourDateTime, string ticketCode)
    {
        bool removedAnyVisitor = false;

        // No need to convert tourHour; we are already getting a DateTime object as tourDateTime.
        if (TourSlots.TryGetValue(tourDateTime, out List<Visitor> visitors))
        {
            // Use a loop to continuously find and remove visitors with the matching ticketCode
            var visitorToRemove = visitors.FirstOrDefault(v => v.TicketCode == ticketCode);
            while (visitorToRemove != null)
            {
                visitors.Remove(visitorToRemove);
                removedAnyVisitor = true;

                // Look for the next visitor with the same ticketCode, if any
                visitorToRemove = visitors.FirstOrDefault(v => v.TicketCode == ticketCode);
            }
        }

        return removedAnyVisitor;
    }



    public void SaveGuidedToursToFile()
    {
        // Filter out past tours before saving
        var filteredTourData = TourSlots
            .Where(entry => entry.Key.Date >= DateTime.Today) // Keep only today and future tours
            .ToDictionary(
                entry => entry.Key.ToString("yyyy-MM-dd HH:mm"), // Format the date as a string
                entry => entry.Value.Select(visitor => new { visitor.Name, visitor.TicketCode }).ToList()
            );

        string filePath = "./JSON-Files/guidedTours.json";
        string json = JsonConvert.SerializeObject(filteredTourData, Formatting.Indented);

        // First, update the guidedTours.json file with the current state
        File.WriteAllText(filePath, json);

        // Then, archive this updated file
        ArchiveGuidedToursFile();
    }


    public void LoadToursFromFile(string filePath)
    {
        try
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                var loadedTourSlots = JsonConvert.DeserializeObject<Dictionary<string, List<Visitor>>>(json);

                if (loadedTourSlots != null)
                {
                    ClearTourSlots();

                    // Convert the string keys back to DateTime and initialize tours for today if needed
                    foreach (var slot in loadedTourSlots)
                    {
                        DateTime slotTime = DateTime.ParseExact(slot.Key, "yyyy-MM-dd HH:mm", CultureInfo.InvariantCulture);
                        TourSlots[slotTime] = slot.Value;
                    }

                    // Check if there are slots for today and initialize if not
                    if (!TourSlots.Keys.Any(dt => dt.Date == DateTime.Today))
                    {
                        //InitializeTourSlotsForToday();
                    }
                }
                else
                {
                    // Console.WriteLine("Couldn't find tours so initializing for today.");
                    //InitializeTourSlotsForToday();
                }
            }
            else
            {
                // If the file does not exist, initialize tours for today
                //InitializeTourSlotsForToday();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading tours: {ex.Message}");
            // Optionally initialize tours for today even in case of error, or handle differently
            //InitializeTourSlotsForToday();
        }
    }


    public void ArchiveGuidedToursFile()
    {
        try
        {
            string targetBasePath = @"C:\Users\moham\Desktop\School\Project-B\Project-B\ReservationSystem";
            string jsonFilesFolderPath = Path.Combine(targetBasePath, "JSON-Files");
            string archiveFolderPath = Path.Combine(jsonFilesFolderPath, "GuidedTours");
            string sourceFilePath = Path.Combine(jsonFilesFolderPath, "guidedTours.json");
            string archiveFilePath = Path.Combine(archiveFolderPath, $"guidedTours_{DateTime.Now:yyyyMMdd}.json");

            if (!Directory.Exists(archiveFolderPath))
            {
                Directory.CreateDirectory(archiveFolderPath);
            }

            if (File.Exists(sourceFilePath))
            {
                // Check if the archive file already exists
                if (File.Exists(archiveFilePath))
                {
                    // Try to delete the existing file
                    File.Delete(archiveFilePath);
                }

                // Copy the file to the archive
                File.Copy(sourceFilePath, archiveFilePath, true);
                // Console.WriteLine($"Archive updated successfully at: {archiveFilePath}");
            }
            else
            {
                Console.WriteLine("Source file not found. Cannot archive.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during archiving: {ex.Message}");
        }
    }


    public bool UpdateMaxCapacity(int newCapacity)
    {
        return true;
    }

    public bool ChangeTourTime(int oldTourHour)
    {
        return true;
    }*/

    // Static class

    public static string tourJSONpath = "./JSON-Files/GuidedTours.json";
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

    // Adds the given tour to the Json file :
    //  - Checks if the tour has correct format
    //  - Checks if the tour is in the future or in the past
    //  - Checks if the tour is already in the file, based on the _tourId
    //  - Adds the given tour to the list of tours in the static class
    //  - Updates the Json file with the list of tours in the static class
    public static void AddTourToJSON(GuidedTour tour)
    {
        GuidedTour._updateCurrentTours();
        TimeOnly tourTime = TimeOnly.FromDateTime(tour.StartTime);
        DateOnly tourDate = DateOnly.FromDateTime(tour.StartTime);
        bool tourAlreadyInFile = _checkIfInFile(tour);
        bool allowedId = tour.TourId >= 100000000 && tour.TourId <= 999999999;
        bool allowedTime = _checkIfAllowedTime(tourTime);
        bool allowedDate = _checkIfAllowedDate(tourDate);
        if (!allowedTime || !allowedDate || tourAlreadyInFile || !allowedId)
        {
            return;
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
    public static void EditTourInJSON(GuidedTour oldTour, GuidedTour newTour)
    {
        GuidedTour._updateCurrentTours();
        bool oldTourExsists = GuidedTour.CurrentTours.Contains(oldTour);
        bool bothToursSameId = oldTour.TourId == newTour.TourId;

        if (oldTourExsists || !bothToursSameId)
        {
            return;
        }

        GuidedTour.RemoveTourFromJSON(oldTour);
        GuidedTour.AddTourToJSON(newTour);
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

    // Generates a unique random int number between 100000000 and 999999999, this number is to be used as the id of new tours
    //  - Uses a random seed to generate and return a number in the range
    //  - Checks if the generated id already an id of a different tour
    private static int _generateUniqueId()
    {
        Random gen = new();
        bool idIsUnique = false;
        int uniqueId = 0;
        List<int> ids = new();

        foreach (GuidedTour tour in GuidedTour.CurrentTours)
        {
            ids.Add(tour.TourId);
        }

        while (idIsUnique == false)
        {
            int newId = gen.Next(100000000, 999999999);
            if (!ids.Contains(newId))
            {
                uniqueId = newId;
                idIsUnique = true;
            }
        }

        return uniqueId;
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
            day.AddDays(dayIndex - 1);

            if (day.DayOfWeek == DayOfWeek.Monday)
            {
                mondays.Add(day);
            }
        }
        return mondays;
    }

    private static bool _checkIfAllowedTime(TimeOnly time)
    {
        List<int> allowedHours = new List<int>() { 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        bool allowed = true;

        if (time.Minute != 0 && time.Minute != 20 && time.Minute != 40)
        {
            allowed = false;
        }
        if (!allowedHours.Contains(time.Hour))
        {
            allowed = false;
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
}