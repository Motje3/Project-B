using Newtonsoft.Json;

public class GuidedTour
{
    public int StartTime { get; private set; }
    public int EndTime { get; private set; }
    public int MaxCapacity { get; private set; }
    public Dictionary<int, List<Visitor>> TourSlots { get; private set; }

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
            string json = File.ReadAllText(filePath);
            dynamic settings = JsonConvert.DeserializeObject(json);

            StartTime = settings?.StartTime ?? 9; // Provide a default value of 9 if null
            EndTime = settings?.EndTime ?? 17;    // Provide a default value of 17 if null
            MaxCapacity = settings?.MaxCapacity ?? 13; // Provide a default value of 13 if null
        }
        else
        {
            Console.WriteLine("Tour settings file not found. Using default settings.");
            // Set default values
            StartTime = 9;
            EndTime = 17;
            MaxCapacity = 13;
        }
    }


    private void InitializeTourSlots()
    {
        TourSlots = new Dictionary<int, List<Visitor>>();
        for (int hour = StartTime; hour <= EndTime; hour++)
        {
            TourSlots.Add(hour, new List<Visitor>());
        }
    }

    public void ListAvailableTours(int numberOfPeopleAttemptingToJoin)
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


    public bool JoinTour(int hour, Visitor visitor)
    {
        // Ensure the hour is within the start and end times.
        if (hour < StartTime || hour > EndTime)
        {
            Console.WriteLine("The chosen hour is outside the tour operation hours.");
            return false;
        }

        // If we get here, the tour is either full or the hour doesn't have a slot.
        if (!TourSlots.ContainsKey(hour))
        {
            Console.WriteLine("There is no tour at the chosen hour.");
        }

        // Check if the slot exists and is not full.
        if (TourSlots.ContainsKey(hour) && TourSlots[hour].Count < MaxCapacity)
        {
            TourSlots[hour].Add(visitor);
            return true;
        }

        return false;
    }


    public bool UpdateVisitorTour(string ticketCode, int newTourHour)
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
            if (TourSlots.TryGetValue(newTourHour, out List<Visitor> newSlot) && newSlot.Count + visitorsToUpdate.Count <= MaxCapacity)
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
                Console.WriteLine("All visitors updated successfully.");
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

    public bool RemoveVisitorFromTour(int tourHour, string ticketCode)
    {
        bool removedAnyVisitor = false;

        if (TourSlots.TryGetValue(tourHour, out List<Visitor> visitors))
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
            entry => entry.Key,
            entry => entry.Value.Select(visitor => new { visitor.Name, visitor.TicketCode }).ToList()
        );

        string filePath = "./JSON-Files/guidedTours.json"; // Specify the path to your JSON file
        string json = JsonConvert.SerializeObject(tourData, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public void LoadToursFromFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            var loadedTourSlots = JsonConvert.DeserializeObject<Dictionary<int, List<Visitor>>>(json);

            if (loadedTourSlots != null)
            {
                foreach (var slot in loadedTourSlots)
                {
                    if (TourSlots.ContainsKey(slot.Key))
                    {
                        TourSlots[slot.Key] = slot.Value; // Update existing tours with loaded data
                    }
                }
            }
        }
    }

    public bool UpdateMaxCapacity(int newCapacity)
    {
        if (newCapacity <= 0)
        {
            Console.WriteLine("Capacity must be greater than 0.");
            return false;
        }

        // Update the MaxCapacity.
        MaxCapacity = newCapacity;
        Console.WriteLine($"Max capacity updated to {newCapacity}.");
        return true;
    }

    public bool ChangeTourTime(int oldTourHour)
    {
        // Check if the old tour exists and the new tour hour is valid

        // Move visitors from the old tour to the new tour hour
        TourSlots.Remove(oldTourHour);  // Remove the old tour slot

        // Optionally, save changes to file
        SaveGuidedToursToFile();
        return true;
    }
}