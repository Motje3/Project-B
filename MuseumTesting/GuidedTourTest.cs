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
    // Adding 1 minutes before first allowed tour time
    [DataRow(59, false)]
    // Adding 1 minute after first allowed tour time
    [DataRow(61, false)]
    // Adding 1 minutes before last allowed tour time (16:19)
    [DataRow(60 * 8 + 19, false)]
    // Adding at last allowed tour time (16:20)
    [DataRow(60 * 8 + 20, true)]
    // Adding 1 minutes after last allowed tour time (16:21)
    [DataRow(60 * 8 + 21, false)]
    // Adding 20 minutes after last allowed tour time (16:40)
    [DataRow(60 * 8 + 40, false)]
    public void TestAddTour(int minutesToAdd, bool expected)
    {
        // Arrange

        bool yesterdayWasMonday = DateTime.Today.AddDays(-1).DayOfWeek == DayOfWeek.Monday;
        bool todayIsMonday = DateTime.Today.DayOfWeek == DayOfWeek.Monday;
        bool tommorowIsMonday = DateTime.Today.AddDays(1).DayOfWeek == DayOfWeek.Monday;
        // According to the website of the museum there are no tours on mondays, so the test should stop if it is run before, on or after a monday
        /*if (yesterdayWasMonday || todayIsMonday || tommorowIsMonday)
            return;*/

        // There ARE tours on 4th of every month
        DateTime startTime1 = new(nowYear, 1, 4, 8, 0, 0);
        startTime1 = startTime1.AddMinutes(minutesToAdd);
        GuidedTour toBeAdded1 = new(startTime1);

        DateTime startTime2 = new(nowYear, 12, 4, 8, 0, 0);
        startTime2 = startTime2.AddMinutes(minutesToAdd);
        GuidedTour toBeAdded2 = new(startTime2);

        // Act
        GuidedTour.AddTourToJSON(toBeAdded1);
        GuidedTour.AddTourToJSON(toBeAdded2);
        List<GuidedTour> currentTours = _readJSON();
        bool actual = currentTours.Count == 2;

        // Assert
        // Assert if actually added succesfully
        Assert.AreEqual(expected, actual);

        bool startTime1InPast = DateTime.Compare(DateTime.Now, startTime1) == 1;
        bool startTime1InFuture = DateTime.Compare(DateTime.Now, startTime1) == -1;
        if (startTime1InPast && expected == true)
        {
            string toBeAddedAsString = JsonConvert.SerializeObject(toBeAdded1);
            string actualTourAsString = JsonConvert.SerializeObject(GuidedTour.CompletedTours[0]);
            // Assert if added in CurrentTours or in CompletedTours succesfully
            Assert.AreEqual(toBeAddedAsString, actualTourAsString);
        }
        else if (startTime1InFuture && expected == true)
        {
            string toBeAddedAsString = JsonConvert.SerializeObject(toBeAdded2);
            string actualTourAsString = JsonConvert.SerializeObject(GuidedTour.CurrentTours[0]);
            // Assert if added in CurrentTours or in CompletedTours succesfully
            Assert.AreEqual(toBeAddedAsString, actualTourAsString);
        }
    }

    [TestMethod]
    public void TestAddTwoToursSameTime()
    {
    // Arrange
        DateTime startTime1 = new(nowYear, 1, 4, 9, 0, 0);
        GuidedTour toBeAdded1 = new(startTime1);
        toBeAdded1.Completed = true;
        GuidedTour toBeAdded2 = new(startTime1);
        toBeAdded2.Completed = true;
        List<GuidedTour> bothTours = new List<GuidedTour>(){toBeAdded1,toBeAdded2};
        string bothToursAsString = JsonConvert.SerializeObject(bothTours);

    // Act
        GuidedTour.AddTourToJSON(toBeAdded1);
        GuidedTour.AddTourToJSON(toBeAdded2);

    // Assert
        List<GuidedTour> currentTours = _readJSON();
        bool actual = currentTours.Count == 2;
        Assert.IsTrue(actual);

        string actualTourAsString = JsonConvert.SerializeObject(currentTours);
        Assert.AreEqual(bothToursAsString, actualTourAsString);
    }

    [TestMethod]
    public void TestDeleteTour()
    {
    // Arrange
        DateTime startTime = new(nowYear, 1, 4, 9, 0, 0);
        GuidedTour toBeAdded = new(startTime);
        GuidedTour.AddTourToJSON(toBeAdded);

    // Act
        GuidedTour.DeleteTourFromJSON(toBeAdded);
        List<GuidedTour> currentTours = _readJSON();
    // Assert
        Assert.IsTrue(currentTours[0].Deleted == true);
        Assert.IsTrue(GuidedTour.CurrentTours.Count == 0);
        Assert.IsTrue(GuidedTour.CompletedTours.Count == 0);
        Assert.IsTrue(GuidedTour.DeletedTours.Count == 1);
    }

    [TestMethod]
    public void TestEditTour()
    {
    // Arrange
        DateTime startTime = new(nowYear, 1, 4, 9, 0, 0);
        GuidedTour toBeEditted = new(startTime);
        GuidedTour.AddTourToJSON(toBeEditted);

        GuidedTour editted = toBeEditted.Clone();
        editted.Deleted = true;

    // Act
        GuidedTour.EditTourInJSON(toBeEditted, editted);
        List<GuidedTour> currentTours = _readJSON();
    // Assert
        Assert.IsTrue(currentTours.Count == 1);
        string savedTour = JsonConvert.SerializeObject(currentTours[0]);
        string beforeSavedTour = JsonConvert.SerializeObject(editted);
        Assert.AreEqual(beforeSavedTour, savedTour);
    }

    [TestMethod]
    public void AddVisitor_GivenGuide_VerifyAssignedGuide()
    {
        // Arrange
        var guidedTour = new GuidedTour(DateTime.Now);
        var guide = new Guide("GUIDE123",guidedTour.TourId);

        // Act
        guidedTour.AddVisitor(guide);

        // Assert
        Assert.AreEqual(guide, guidedTour.AssignedGuide);
    }

    [TestMethod]
    public void AddVisitor_GivenTourAtMaxCapacity()
    {
        // Arrange
        var guidedTour = new GuidedTour(DateTime.Now);
        for (int i = 0; i < guidedTour.MaxCapacity; i++)
        {
            guidedTour.ExpectedVisitors.Add(new Visitor("123"));
        }
        var visitor = new Visitor("456");

        // Act
        guidedTour.AddVisitor(visitor);

        // Assert
        Assert.IsFalse(guidedTour.ExpectedVisitors.Contains(visitor));
    }

    [TestMethod]
    public void AddVisitor_GivenDeletedTour()
    {
        // Arrange
        var guidedTour = new GuidedTour(DateTime.Now);
        guidedTour.Deleted = true;
        var visitor = new Visitor("123");

        // Act
        guidedTour.AddVisitor(visitor);

        // Assert
        Assert.IsFalse(guidedTour.ExpectedVisitors.Contains(visitor));
    }

    [TestMethod]
    public void AddVisitor_GivenCompletedTour()
    {
        // Arrange
        var guidedTour = new GuidedTour(DateTime.Now);
        guidedTour.Completed = true;
        var visitor = new Visitor("123");

        // Act
        guidedTour.AddVisitor(visitor);

        // Assert
        Assert.IsFalse(guidedTour.ExpectedVisitors.Contains(visitor));
    }

        [TestMethod]
    public void TestVisitorCancelsReservation()
    {
        // Arrange
        DateTime startTime = new(nowYear, 1, 4, 9, 0, 0);
        GuidedTour tour = new GuidedTour(startTime);
        Guide guide = new Guide("GUIDE123", tour.TourId);
        Visitor visitor1 = new Visitor("TICKET456");
        Visitor visitor2 = new Visitor("TICKET789");

        
        tour.AssignedGuide = guide; // Assuming guide is properly set as a Guide object
        tour.ExpectedVisitors.Add(visitor1);
        
        GuidedTour.AddTourToJSON(tour);

        // Act
        // Visitor1 cancels their reservation
        tour.RemoveVisitor(visitor1);
        // Try to remove a visitor that isn't in the tour
        tour.RemoveVisitor(visitor2);

        // Assert
        List<GuidedTour> updatedTours = _readJSON();

        Assert.IsTrue(updatedTours[0].ExpectedVisitors.Count == 0);
    }

    [TestMethod]
    public void TestVisitorTransferReservation_succes()
    {
        // Arrange
        DateTime startTime1 = new(nowYear, 12, 30, 9, 0, 0);
        DateTime startTime2 = new(nowYear, 12, 30, 9, 20, 0);
        GuidedTour tour1 = new GuidedTour(startTime1);
        GuidedTour tour2 = new GuidedTour(startTime2);
        GuidedTour.AddTourToJSON(tour1);
        GuidedTour.AddTourToJSON(tour2);
        
        Visitor visitor1 = new Visitor("TICKET123");
        tour1.AddVisitor(visitor1);

        // Act
        tour1.TransferVisitor(visitor1, tour2);
        List<GuidedTour> tours = _readJSON();

        // Assert
        Assert.IsTrue(tour1.ExpectedVisitors.Count == 0);
        Assert.IsTrue(tour2.ExpectedVisitors.Count == 1);
        Assert.IsTrue(tours[0].ExpectedVisitors.Count == 0);
        Assert.IsTrue(tours[1].ExpectedVisitors.Count == 1);
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
