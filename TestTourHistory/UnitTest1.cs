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
            toursHistory.LogReservation("DEV1", "CODE-0001", TourTime, false); // name, ticketcode, tourtime, IsCancel
            toursHistory.LogReservation("DEV1", "CODE-0001", TourTime, true);
            bool result = File.Exists(textPath);
            Assert.IsTrue(result);

            DateTime logtime = DateTime.Now;
            // string strtourtime = TourTime.ToString("HH:mm");
            string strlogtime = logtime.ToString("HH:mm");
            string matchRegistration = $"{strlogtime}: CODE-0001 DEV1 registered for tour on {TourTime}"; // match for index [-2] of text file // false
            string matchCancel = $"{strlogtime}: CODE-0001 DEV1 canceled his/her tour on {TourTime}"; // match for index [-1] of text file // true

            string[] lines = File.ReadAllLines(textPath);
            string lastLine = lines[^1]; // Python equivalent of lines[-1]
            string secondLastLine = lines[^2]; // Python equivalent of lines[-2]

            Assert.AreEqual(matchRegistration, secondLastLine);
            Assert.AreEqual(matchCancel, lastLine);
        }
        [TestMethod]
        public void TestLogGuidedTour()
        {
            ToursHistory toursHistory = new ToursHistory();
            DateTime currentDate = DateTime.Today; 
            DateTime TourTime = DateTime.Now;
            string textPath = $"./Logs/GuidedTourLog/{currentDate:dd-MM-yyyy}_GuidedTourLog.txt";
            toursHistory.LogGuidedTour("DEV1", "CODE-0001", TourTime);  // name, ticketcode, tourtime
            toursHistory.LogGuidedTour("DEV2", "CODE-0002", TourTime);
            bool result = File.Exists(textPath);
            Assert.IsTrue(result);

            string[] lines = File.ReadAllLines(textPath);
            string lastLine = lines[^1]; // Python equivalent of lines[-1]
            string secondLastLine = lines[^2]; // Python equivalent of lines[-2]

            DateTime logtime = DateTime.Now;
            string strtourtime = TourTime.ToString("HH:mm");
            string strlogtime = logtime.ToString("HH:mm");
            string checkMatch1 = $"{strlogtime}: TourTime[{strtourtime}] CODE-0001 DEV1 has joined the tour";  // match for index [-2] of text file
            string checkMatch2 = $"{strlogtime}: TourTime[{strtourtime}] CODE-0002 DEV2 has joined the tour";  // match for index [-1] of text file

            Assert.AreEqual(checkMatch1, secondLastLine);
            Assert.AreEqual(checkMatch2, lastLine);
        }
    }
}