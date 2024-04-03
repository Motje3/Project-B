namespace MuseumTesting;
using Newtonsoft.Json;

[TestClass]
public class EntryCodeTest
{
    private List<string> _EntryCodesAtTheStart = new() { };
    private const string JSONPath = "./JSON-Files/sample_codes.json";

    [TestInitialize]
    public void Setup()
    {
        _rememberJSON();
    }
    [TestCleanup]
    public void Cleanup()
    {
        _forgetJSON();
    }

    [DataTestMethod]
    [DataRow("123456", "123456", true)]
    [DataRow("123456", "000000", false)]
    [DataRow("ABCDEF", "ABCDEF", true)]
    [DataRow("ABCDEF", "XXXXXX", false)]
    public void TestCode(string testCode, string codeToAdd, bool expected)
    {
        // Arrange
        _addNewCode(codeToAdd);
        EntreeCodeValidator validator = new();
        // Act
        bool actual = validator.IsCodeValid(testCode);
        // Assert
        Assert.AreEqual(expected, actual);
    }



    // Saves the current contents of sample_codes.json to the variable _EntryCodesAtTheStart
    private void _rememberJSON()
    {
        using (StreamReader reader = new StreamReader(JSONPath))
        {
            string jsonData = reader.ReadToEnd();
            _EntryCodesAtTheStart = JsonConvert.DeserializeObject<List<string>>(jsonData);
        }
    }

    // Reverts the contents of sample_coes.json back to what it was when rememberJSON() was used
    private void _forgetJSON()
    {
        using (StreamWriter writer = new StreamWriter(JSONPath))
        {
            string List2Json = JsonConvert.SerializeObject(_EntryCodesAtTheStart);
            writer.Write(List2Json);
        }
    }

    // To be used for testing purposes, adds an int code to sample_codes.json
    private void _addNewCode(string code)
    {
        using (StreamWriter writer = new StreamWriter(JSONPath))
        {
            List<string> toWrite = new(_EntryCodesAtTheStart);
            toWrite.Add(code);

            string List2Json = JsonConvert.SerializeObject(toWrite);
            writer.Write(List2Json);
        }
    }

    private List<string> _readJSON()
    {
        using (StreamReader reader = new StreamReader(JSONPath))
        {
            string jsonData = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<string>>(jsonData);
        }
    }
}
