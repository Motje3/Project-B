namespace MuseumTesting;
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
    [TestMethod]
    public void TestJoinTour()
    {
        DateTime starttime = new DateTime (2024, 12 ,30, 17, 00, 00); 
        int duration = 20;
        DateTime endtime = starttime.AddMinutes(duration);
        GuidedTour tour1 = new GuidedTour(starttime);
        Visitor John = new Visitor("John Doe", "111");
        Visitor Alica = new Visitor("Alica Doe", "222");
        tour1.PresentVisitors = new List<Visitor>() {John};  
        tour1.ExpectedVisitors = new List<Visitor>() {Alica};

        // true
        bool result1 = tour1.PresentVisitors.Contains(John);
        bool result2 = tour1.ExpectedVisitors.Contains(Alica);
        bool result3 = tour1.PresentVisitors.Contains(Alica);

        // false
        bool result4 = tour1.ExpectedVisitors.Contains(John);

        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        
        Assert.IsTrue(result3);
        Assert.IsFalse(result4);
    }
}
