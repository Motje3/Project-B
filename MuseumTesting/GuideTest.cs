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
        DateTime starttime = new DateTime(2024, 5, 4, 9, 00, 00);
        GuidedTour tour1 = new GuidedTour(starttime);
        GuidedTour.AddTourToJSON(tour1);
        Guide John = new Guide("111", tour1.TourId);

        tour1.AddVisitor(John);
        Visitor Alica = new Visitor("222");
        Visitor Ben = new("333");
        tour1.AddVisitor(Alica);

        John.CheckInVisitor(Alica);  // add 
        John.CheckInVisitor(Alica);  // do twice to check if it is NOT creating dublicate expected visitor
        John.CheckInVisitor(Ben);

        List<GuidedTour> currTours = _readJSON();
        var currTour = currTours[0]; // JSON[0] 
        var PresentVisitors = currTour.PresentVisitors; // JSON[0] => PresentVisitors
        var visitor = PresentVisitors[0]; // access visitor object by index
        int Len = PresentVisitors.Count; // should be only one in list

        Assert.AreEqual(visitor.TicketCode, "222"); // check matching ticketcode
        Assert.AreEqual(Len, 1); // check if there is no duplicated or unallowed visitors
    }

    [TestMethod]
    public void CompleteTour_Deleted_return_early()
    {
        // Arrange
        DateTime startTime = new(2030, 1, 4, 9, 0, 0);
        GuidedTour existingTour = new GuidedTour(startTime)
        { 
            TourId = Guid.NewGuid(), Deleted = true 
        };
        Guide guide = new Guide("456",existingTour.TourId);
        guide.AssingedTourId = existingTour.TourId;
        existingTour.AddVisitor(guide);

        GuidedTour.AddTourToJSON(existingTour);

        // Act
        guide.CompleteTour();
        List<GuidedTour> tours = _readJSON();

        // Assert
        Assert.IsTrue(existingTour.Completed == false);
        Assert.IsTrue(tours[0].Completed == false);
    }

    [TestMethod]
    public void CompleteTour_Completed_return_true()
    {
        // Arrange
        DateTime startTime = new(2030, 1, 4, 9, 0, 0);
        GuidedTour existingTour = new GuidedTour(startTime)
        {
            TourId = Guid.NewGuid(), Deleted = false 
        };
        Guide guide = new Guide("789",existingTour.TourId);
        guide.AssingedTourId = existingTour.TourId;
        existingTour.AddVisitor(guide);

        // Act
        guide.CompleteTour();
        List<GuidedTour> tours = _readJSON();

        // Assert
        // Verify that the tour is marked as completed
        Assert.IsTrue(tours[0].Completed == true);
    }






    // optional JSON Methods for tests
    // Reverts the contents of GuidedTours.json back to what it was when rememberJSON() was used
    private void _revertJSON()
    {
        using (StreamWriter writer = new StreamWriter(JSONPath))
        {
            string List2Json = JsonConvert.SerializeObject(_toursAtTheStart, Formatting.Indented);
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
            string List2Json = JsonConvert.SerializeObject(empty, Formatting.Indented);
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