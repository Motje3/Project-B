namespace MuseumTesting;
using Newtonsoft.Json;
// StartTime = startTime;
// Duration = 20; // 20 minutes
// EndTime = startTime.AddMinutes(Duration);
// MaxCapacity = 13;
// TourId = Guid.NewGuid();
// Completed = false;
// Deleted = false;
[TestClass]
public class GuideTest
{
    private const string JSONPath = "./JSON-Files/GuidedTours.json";
    private List<GuidedTour> _toursAtTheStart = new();


    [TestInitialize]
    public void Setup()
    {
        _rememberJSON();
        _emptyJSON();
    }
    [TestCleanup]
    public void Cleanup()
    {
        _revertJSON();
    }

    [TestMethod]
    public void TestJoinTour()
    {
        DateTime starttime = new DateTime (2024, 5 ,4, 9, 00, 00); 
        GuidedTour tour1 = new GuidedTour(starttime);
        GuidedTour.AddTourToJSON(tour1);
        Guide John = new Guide("John Doe", "111", tour1.TourId);
        
        tour1.AddVisitor(John);
        Visitor Alica = new Visitor("Alica Doe", "222");
        
        tour1.AddVisitor(Alica);
        
        John.CheckInVisitor(Alica);  // add 
        John.CheckInVisitor(Alica);  // do twice to check if it is NOT creating dublicate expected visitor

        List<GuidedTour> currTours = _readJSON();
        var currTour = currTours[0]; // JSON[0] 
        var ExpectedVistor = currTour.ExpectedVisitors; // JSON[0] => ExpectedVisitor
        var visitor = ExpectedVistor[0]; // access visitor object by index
        int Len = ExpectedVistor.Count; // should be only one in list

        Assert.AreEqual(visitor.Name, "Alica Doe"); // check matching name
        Assert.AreEqual(visitor.TicketCode, "222"); // check matching ticketcode
        Assert.AreEqual(Len, 1); // check if there is no dublicated
    }
    
    // optional JSON Methods for tests
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
