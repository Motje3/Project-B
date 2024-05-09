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



    
}
