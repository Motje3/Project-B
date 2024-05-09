namespace MuseumTesting;
using Newtonsoft.Json;

[TestClass]
public class EntryCodeTest
{
    private List<string> _EntryCodesAtTheStart = new() { };
    private const string JSONPath = "./DataLayer/JSON-Files/sample_codes.json";

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
    // Correct code numbers
    [DataRow("123456", "123456", true)]
    // Wrong code numbers
    [DataRow("123456", "000000", false)]
    // Correct code letters
    [DataRow("ABCDEF", "ABCDEF", true)]
    // Wrong code letters
    [DataRow("ABCDEF", "XXXXXX", false)]
    // Wrong code check for case sensivity
    [DataRow("ABCDEF", "abcdef", false)]
    // Correct code mixed
    [DataRow("abc456!@#", "abc456!@#", true)]
    // Wrong code mixed
    [DataRow("abc456!@#", "xxx000???", false)]

    // Takes a code in form of a string to test, a code which will be added to json to test against and expected bool
    public void TestCode(string testCode, string codeToAdd, bool expected)
    {
        // Arrange
        _addNewCode(codeToAdd);
        // Act
        bool actual = Ticket.IsCodeValid(testCode);
        // Assert
        //Assert.AreEqual(expected, actual);
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

}
