namespace MuseumTesting;
using Newtonsoft.Json;

[TestClass]
public class GuidedTourTesting
{
    [TestInitialize]
    public void Setup()
    {
        _rememberJSON();
        _emptyJSON();
        nowYear = DateTime.Now.Year;
    }
    [TestCleanup]
    public void Cleanup()
    {
        _revertJSON();
    }

    private List<GuidedTour> _toursAtTheStart = new() { };
    private const string JSONPath = "./JSON-Files/GuidedTours.json";
    private int nowYear;
 
    
    [DataTestMethod]
    [DataRow(0, false)]
    [DataRow(20, false)]
    [DataRow(40, false)]
    [DataRow(60, true)]
    [DataRow(80, true)]
    // Adding 1 minute before first allowed tour time
    [DataRow(59, false)]
    // Adding 1 minute after first allowed tour time
    [DataRow(61, false)]
    // Adding 1 minute before last allowed tour time (16:39)
    [DataRow(60*8+39, false)]
    // Adding at last allowed tour time (16:40)
    [DataRow(60*8+40, true)]
    // Adding 1 minute after last allowed tour time (16:41)
    [DataRow(60*8+41, false)]
    // Adding 20 minute after last allowed tour time (17:00)
    [DataRow(60*9, false)]

    public void TestAddingTours(int minutesToAdd, bool expected)
    {
    // Arrange
        // There ARE tours on 4th of every month
        DateTime startTime = new(nowYear, 1, 4, 8,0,0);
        startTime = startTime.AddMinutes(minutesToAdd);
        GuidedTour toBeAdded = new(startTime);

    // Act
        GuidedTour.AddTourToJSON(toBeAdded);
        List<GuidedTour> currentTours = _readJSON();
        bool actual = currentTours.Count == 1;

    // Assert
        Assert.AreEqual(expected, actual);
    }


    [TestMethod]
    public void TestCompleteToursProperty()
    {
    // Arrange
        bool yesterdayWasMonday = DateTime.Today.AddDays(-1).DayOfWeek == DayOfWeek.Monday;
        bool todayIsMonday = DateTime.Today.DayOfWeek == DayOfWeek.Monday;
        bool tommorowIsMonday = DateTime.Today.AddDays(1).DayOfWeek == DayOfWeek.Monday;
        // According to the website of the museum there are no tours on mondays, so the test should stop if it is run before, on or after a monday
        if (yesterdayWasMonday || todayIsMonday || tommorowIsMonday)
            return;
    
    // Act

    // Assert
        

    }













































    // Reverts the contents of GuidedTours.json back to what it was when rememberJSON() was used
    private void _revertJSON()
    {
        using (StreamWriter writer = new StreamWriter(JSONPath))
        {
            string List2Json = JsonConvert.SerializeObject(_toursAtTheStart,Formatting.Indented);
            writer.Write(List2Json);
        }
    }

    // Saves the current contents of GuidedTours.json to the variable _toursAtTheStart
    private void _rememberJSON()
    {
        using (StreamReader reader = new StreamReader(JSONPath))
        {
            string jsonData = reader.ReadToEnd();
            _toursAtTheStart = JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);
        }
    }

    private void _emptyJSON()
    {
        using (StreamWriter writer = new StreamWriter(JSONPath))
        {
            List<GuidedTour> empty = new();
            string List2Json = JsonConvert.SerializeObject(empty,Formatting.Indented);
            writer.Write(List2Json);
        }
    }

    private List<GuidedTour> _readJSON()
    {
        using (StreamReader reader = new StreamReader(JSONPath))
        {
            string jsonData = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<List<GuidedTour>>(jsonData);
        }
    }
}
