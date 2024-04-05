using Newtonsoft.Json;
using System.Globalization;


public class GuidedTour
{
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public TimeSpan TourInterval { get; private set; } = TimeSpan.FromMinutes(20); // Default to 20 minutes
    public int MaxCapacity { get; private set; }
    public Dictionary<DateTime, List<Visitor>> TourSlots { get; private set; }

    public GuidedTour()
    {
        LoadTourSettings();
        InitializeTourSlots();
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

                string startTimeString = settings?.StartTime ?? "2024-04-05T09:00:00";
                string endTimeString = settings?.EndTime ?? "2024-04-05T17:00:00";
                StartTime = DateTime.Parse(startTimeString);
                EndTime = DateTime.Parse(endTimeString);
                TourInterval = TimeSpan.FromMinutes((int)(settings?.TourInterval ?? 20));
                MaxCapacity = (int)(settings?.MaxCapacity ?? 13);
            }
            catch (FormatException ex)
            {
                Console.WriteLine($"Error parsing tour settings: {ex.Message}");
                // Handle the error appropriately, perhaps setting default values
            }
        }
        else
        {
            Console.WriteLine("Tour settings file not found. Using default settings.");
            StartTime = DateTime.Parse("2024-04-05T09:00:00");
            EndTime = DateTime.Parse("2024-04-05T17:00:00");
            TourInterval = TimeSpan.FromMinutes(20);
            MaxCapacity = 13;
        }
    }




    private void InitializeTourSlots()
    {
        TourSlots = new Dictionary<DateTime, List<Visitor>>();
        DateTime slotTime = StartTime;

        while (slotTime < EndTime)
        {
            TourSlots.Add(slotTime, new List<Visitor>());
            slotTime = slotTime.Add(TourInterval);
        }
    }

    public void ListAvailableTours(int numberOfPeopleAttemptingToJoin)
    {
        DateTime now = DateTime.Now;

        Console.WriteLine("Available tour times:");
        foreach (var slot in TourSlots)
        {
            // Ensure the slot is in the future
            if (slot.Key > now)
            {
                // Check if the tour slot plus the new participants does not exceed MaxCapacity
                if ((slot.Value.Count + numberOfPeopleAttemptingToJoin) <= MaxCapacity)
                {
                    Console.WriteLine($"{slot.Key.ToString("h:mm tt")} - {slot.Value.Count} participants");
                }
            }
        }
    }



    public void ListAvailableToursAdmin(int numberOfPeopleAttemptingToJoin)
    {
        Console.WriteLine("Available tour times:");
        foreach (var slot in TourSlots)
        {
            // Check if the tour slot plus the new participants does not exceed MaxCapacity
            if ((slot.Value.Count + numberOfPeopleAttemptingToJoin) <= MaxCapacity)
            {
                Console.WriteLine($"{slot.Key}:00 - {slot.Value.Count} participants");
            }
        }
    }


    public bool JoinTour(DateTime tourTime, Visitor visitor)
    {
        // Ensure the tourTime is within the start and end times.
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
            Console.WriteLine("The chosen tour is full.");
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
        var tourData = TourSlots.ToDictionary(
            entry => entry.Key.ToString("yyyy-MM-dd HH:mm"), // Space instead of 'T'
                                                             // Or, use another character like: entry.Key.ToString("yyyy-MM-dd_HH:mm")
            entry => entry.Value.Select(visitor => new { visitor.Name, visitor.TicketCode }).ToList()
        );

        string filePath = "./JSON-Files/guidedTours.json";
        string json = JsonConvert.SerializeObject(tourData, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }




    public void LoadToursFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            // Adjust the deserialization to handle DateTime keys
            var loadedTourSlots = JsonConvert.DeserializeObject<Dictionary<DateTime, List<Visitor>>>(json);

            if (loadedTourSlots != null)
            {
                // Clear the existing slots and replace them with the loaded data
                TourSlots = loadedTourSlots;
            }
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