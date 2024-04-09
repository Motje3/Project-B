using Newtonsoft.Json;
using System.Globalization;


public class GuidedTour
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan TourInterval { get; private set; }
    public int MaxCapacity { get; private set; }
    public Dictionary<DateTime, List<Visitor>> TourSlots { get; private set; }

    public GuidedTour()
    {
        TourSlots = new Dictionary<DateTime, List<Visitor>>();
        LoadTourSettings();
        InitializeTourSlotsForToday(); // Now it's safe to call this
    }

    private void LoadTourSettings()
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
    }




    private void InitializeTourSlotsForToday()
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
    }


    private void ClearTourSlots()
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
                        InitializeTourSlotsForToday();
                    }
                }
                else
                {
                    // Console.WriteLine("Couldn't find tours so initializing for today.");
                    InitializeTourSlotsForToday();
                }
            }
            else
            {
                // If the file does not exist, initialize tours for today
                InitializeTourSlotsForToday();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while loading tours: {ex.Message}");
            // Optionally initialize tours for today even in case of error, or handle differently
            InitializeTourSlotsForToday();
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
    }
}