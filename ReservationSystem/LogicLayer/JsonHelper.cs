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
        List<GuidedTour> tours = GuidedTour.CompletedTours.Concat(GuidedTour.CurrentTours).ToList().Concat(GuidedTour.DeletedTours).ToList();
        GuidedTour.CurrentTours.Add(tour);
        UpdateTourList(tours, filePath);
    }

    public static void RemoveTour(Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var tourToRemove = tours.Find(t => t.TourId == tourId);
        if (tourToRemove != null)
        {
            bool foundInCurrent = GuidedTour.CurrentTours.Find(t => t.TourId == tourId) != null;
            if (foundInCurrent)
                {
                    GuidedTour.CurrentTours.Remove(tourToRemove);
                    GuidedTour.DeletedTours.Add(tourToRemove);
                }
            else
                {
                    GuidedTour.CompletedTours.Remove(tourToRemove);
                    GuidedTour.DeletedTours.Add(tourToRemove);
                }

            tours = GuidedTour.CompletedTours.Concat(GuidedTour.CurrentTours).ToList().Concat(GuidedTour.DeletedTours).ToList();
            UpdateTourList(tours, filePath);
        }
    }

    public static void EditTour(GuidedTour newTour, Guid tourId, string filePath)
    {
        var tours = LoadTours(filePath);
        var CompletedTourIndex = GuidedTour.CompletedTours.FindIndex(t => t.TourId == tourId);
        var CurrentTourIndex = GuidedTour.CurrentTours.FindIndex(t => t.TourId == tourId);
        
        if (CompletedTourIndex != -1)
            GuidedTour.CompletedTours[CompletedTourIndex] = newTour;
        else if (CurrentTourIndex != -1)
            GuidedTour.CurrentTours[CurrentTourIndex] = newTour;

        tours = GuidedTour.CompletedTours.Concat(GuidedTour.CurrentTours).ToList().Concat(GuidedTour.DeletedTours).ToList();
        UpdateTourList(tours, filePath);
    }
}
