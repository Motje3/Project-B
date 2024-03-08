using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class EntreeCodeValidator
{
    private List<string> validCodes;

    public EntreeCodeValidator(string jsonFilePath)
    {
        string jsonData = File.ReadAllText(jsonFilePath);
        validCodes = JsonConvert.DeserializeObject<List<string>>(jsonData);
    }

    public bool IsCodeValid(string code)
    {
        return validCodes.Contains(code);
    }
}
