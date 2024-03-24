using Newtonsoft.Json;

public class EntreeCodeValidator
{
    private List<string> validCodes;

    public EntreeCodeValidator()
    {
        using (StreamReader reader = new StreamReader("./JSON-Files/sample_codes.json"))
        {
            string jsonData = reader.ReadToEnd();
            validCodes = JsonConvert.DeserializeObject<List<string>>(jsonData);
        }   
    }

    public bool IsCodeValid(string code)
    {
        return validCodes.Contains(code);
    }
}
