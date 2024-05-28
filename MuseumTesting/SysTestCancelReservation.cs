using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestCancelReservation
    {
        [TestCleanup]
        public void CleanUp()
        {
            TourTools.TodaysTours.Clear();
            try
            {
                File.Delete(TourTools.JsonFilePath);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        [TestMethod]
        public void TestVisitorCancelReservationMenuSucceful()
        {
            // Arrange
            string tourChoice = "1";
            string visitorTicketCode = "1234567890";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", "[\"1234567890\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"11:00:00\",\"EndTime\": \"16:15:00\",\"Duration\": 15,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    /* Making a resevertion that will have to be changed and logging out */
                    visitorTicketCode,tourChoice,"Enter,","GETMEOUT",
                    /* Actual input for test */
                    visitorTicketCode,"2","Enter,","GETMEOUT"
                }
            };
            Program.World = world;
                /* Making a resevertion that will have to be changed and logging out */
            Program.Main();
            Tour tourBeforeTransfer = TourTools.FindTourByVisitorTicketCode(visitorTicketCode);

            // Act
            Program.Main();

            // Assert
              // position of the string where the cancel message starts
            int positionCancelMessage = world.LinesWritten.FindIndex(s => s == "Your reservation has been ");
            string actualCancelMessage = world.LinesWritten[positionCancelMessage] + world.LinesWritten[positionCancelMessage + 1] + world.LinesWritten[positionCancelMessage + 2];

            Assert.AreEqual("Your reservation has been cancelled  successfully\n", actualCancelMessage);
        }
    }
}