using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestTourHistory
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestLogReservation()
        {
            ToursHistory toursHistory = new ToursHistory();
            DateTime currentDate = DateTime.Today;         
            DateTime TourTime = DateTime.Now;
            string textPath = $"./Logs/TourReservationLog/{currentDate:dd-MM-yyyy}_TourReservationLog.txt";
            toursHistory.LogReservation("DEV1", "CODE-0001", TourTime, false);
            toursHistory.LogReservation("DEV1", "CODE-0001", TourTime, true);
            bool result = File.Exists(textPath);
            Assert.IsTrue(result);
        }
        [TestMethod]
        public void TestLogGuidedTour()
        {
            ToursHistory toursHistory = new ToursHistory();
            DateTime currentDate = DateTime.Today; 
            DateTime TourTime = DateTime.Now;
            string textPath = $"./Logs/GuidedTourLog/{currentDate:dd-MM-yyyy}_GuidedTourLog.txt";
            toursHistory.LogGuidedTour("DEV1", "CODE-0001", TourTime);
            toursHistory.LogGuidedTour("DEV2", "CODE-0002", TourTime);
            bool result = File.Exists(textPath);
            Assert.IsTrue(result);
        }
    }
}