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
        string json = JsonConvert.SerializeObject(objectToSave);
        File.WriteAllText(filePath, json);
    }
}
