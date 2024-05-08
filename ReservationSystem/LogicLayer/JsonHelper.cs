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
    public static List<Tour> LoadTours(string filePath)
    {
        return LoadFromJson<List<Tour>>(filePath);
    }

    public static void UpdateTourList(List<Tour> tours, string filePath)
    {
        SaveToJson(tours, filePath);
    }



    public static void AddTour(Tour tour, string filePath)
    {
        List<Tour> tours = Tour.CompletedTours.Concat(Tour.CurrentTours).ToList().Concat(Tour.DeletedTours).ToList();
        Tour.CurrentTours.Add(tour);
        UpdateTourList(tours, filePath);
    }

    public static void RemoveTour(Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var tourToRemove = tours.Find(t => t.TourId == tourId);
        if (tourToRemove != null)
        {
            bool foundInCurrent = Tour.CurrentTours.Find(t => t.TourId == tourId) != null;
            if (foundInCurrent)
            {
                Tour.CurrentTours.Remove(tourToRemove);
                Tour.DeletedTours.Add(tourToRemove);
            }
            else
            {
                Tour.CompletedTours.Remove(tourToRemove);
                Tour.DeletedTours.Add(tourToRemove);
            }

            tours = Tour.CompletedTours.Concat(Tour.CurrentTours).ToList().Concat(Tour.DeletedTours).ToList();
            UpdateTourList(tours, filePath);
        }
    }

    public static void EditTour(Tour newTour, Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var CompletedTourIndex = Tour.CompletedTours.FindIndex(t => t.TourId == tourId);
        var CurrentTourIndex = Tour.CurrentTours.FindIndex(t => t.TourId == tourId);

        if (CompletedTourIndex != -1)
            Tour.CompletedTours[CompletedTourIndex] = newTour;
        else if (CurrentTourIndex != -1)
            Tour.CurrentTours[CurrentTourIndex] = newTour;

        tours = Tour.CompletedTours.Concat(Tour.CurrentTours).ToList().Concat(Tour.DeletedTours).ToList();
        UpdateTourList(tours, filePath);
    }
}
