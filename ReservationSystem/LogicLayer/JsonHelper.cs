using Newtonsoft.Json;

public static class JsonHelper
{
    public static T LoadFromJson<T>(string filePath)
    {
        string json = File.ReadAllText(filePath);
        return JsonConvert.DeserializeObject<T>(json);
    }

    public static void SaveToJson<T>(T objectToSave, string filePath)
    {
        string json = JsonConvert.SerializeObject(objectToSave, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    // Specialized method to handle list operations like add, delete, and update for GuidedTour
    public static List<GuidedTour> LoadTours(string filePath)
    {
        return LoadFromJson<List<GuidedTour>>(filePath);
    }

    public static void UpdateTourList(List<GuidedTour> tours, string filePath)
    {
        SaveToJson(tours, filePath);
    }

    public static void AddTour(GuidedTour tour, string filePath)
    {
        var tours = LoadTours(filePath);
        tours.Add(tour);
        UpdateTourList(tours, filePath);
    }

    public static void RemoveTour(Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var tourToRemove = tours.Find(t => t.TourId == tourId);
        if (tourToRemove != null)
        {
            tours.Remove(tourToRemove);
            UpdateTourList(tours, filePath);
        }
    }

    public static void EditTour(GuidedTour newTour, Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var index = tours.FindIndex(t => t.TourId == tourId);
        if (index != -1)
        {
            tours[index] = newTour;
            UpdateTourList(tours, filePath);
        }
    }
}
