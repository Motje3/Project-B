using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestStartingTour
    {
        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                File.Delete(TourTools.JsonFilePath);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        [TestMethod]
        public void TestGuideStartTourWhileEveryonePresent()
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
                    {"./JSON-Files/OnlineTickets.json", $"[\"{visitorTicketCode}\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\"11\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    // Instructions for setup
                    visitorTicketCode,tourChoice,"Enter,","GETMEOUT"
                    // Instructions for test
                    
                }
            };
            Program.World = world;
                // Run main to make a reservation as a visitor
            Program.Main();

            // Act
            Program.Main();
        }
    }
}