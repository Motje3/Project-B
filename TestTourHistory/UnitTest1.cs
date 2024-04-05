using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestTourHistory
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        DateTime TourTime = DateTime.now
        public void TestLogReservation("DEV1", "CODE-0001", TourTime, false)
        {
            ToursHistory.LogReservation("DEV1", "CODE-0001", TourTime, false)
            ToursHistory.LogReservation("DEV1", "CODE-0001", TourTime, false)
        }
    }
}