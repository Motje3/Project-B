using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestVisitorOptions
    {
        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                File.Delete(Tour.JsonFilePath);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        [TestMethod]
        public void TestSeeOptionChangeTourFalse()
        {
            // Arrange
            string visitorTicketCode = "1234567890";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1, 8, 0, 0),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", "[\"1234567890\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"11:00:00\",\"EndTime\": \"16:15:00\",\"Duration\": 15,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    visitorTicketCode,"1","Enter,","GETMEOUT"
                }
            };
            Program.World = world;

            // Act
            Program.Main();

            // Assert
            Assert.IsTrue(!((FakeWorld)Program.World).LinesWritten.Contains("Change my reservation time"));
        }
    }
}