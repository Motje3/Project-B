using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class EntreeCodeValidator
{
    private List<string> validCodes;

    public EntreeCodeValidator()
    {
        // Changed the simple File.ReadAllText way of reading json file into what we learned in class so it's not needed to provide a file path every time
        using (StreamReader reader = new StreamReader("sample_codes.json")) //fileName is "sample_codes.json"
        {
            //string jsonData = File.ReadAllText(jsonFilePath);
            string jsonData = reader.ReadToEnd();
            validCodes = JsonConvert.DeserializeObject<List<string>>(jsonData);
        }
        
        
    }

    public bool IsCodeValid(string code)
    {
        return validCodes.Contains(code);
    }
}
