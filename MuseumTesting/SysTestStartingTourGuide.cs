using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestStartingTour
    {
        [TestCleanup]
        public void CleanUp()
        {
            TourTools.TodaysTours.Clear();
        }

        [TestMethod]
        public void TestGuideStartTourWhileEveryonePresentSuccesful()
        {
            // Arrange
            string tourChoice = "1";
            string visitorTicketCode = "1234567890";
            string guidePassword = "Guide1";
            string guidePasswordInput = "G,u,i,d,e,1,"; 
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"{visitorTicketCode}\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    // Instructions for setup
                    visitorTicketCode,tourChoice,"Enter,","GETMEOUT",
                    // Instructions for test
                    "456",guidePasswordInput,"Enter,","2",visitorTicketCode,"Start","4","GETMEOUT"
                    
                }
            };
            Program.World = world;
                // Run main to make a reservation as a visitor
            Program.Main();

            // Act
            Program.Main();
            // Assert
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("Visitor added to the present visitors list."));
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("Tour has been started successfully."));

            List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(((FakeWorld)Program.World).Files[TourTools.JsonFilePath]);
            Tour tourWithGuide = tours.Where(tour => tour.AssignedGuide.Password == guidePassword).FirstOrDefault();

            Assert.IsTrue(tourWithGuide.PresentVisitors.Select(v => v.TicketCode).Contains(visitorTicketCode));

            Assert.IsTrue(tourWithGuide.Started == true);
        }

        [TestMethod]
        public void TestGuideStartTourTryAddingSamePersonTwice()
        {
            // Arrange
            string tourChoice = "1";
            string visitorTicketCode = "1234567890";
            string guidePassword = "Guide1";
            string guidePasswordInput = "G,u,i,d,e,1,"; 
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"{visitorTicketCode}\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    // Instructions for setup
                    visitorTicketCode,tourChoice,"Enter,","GETMEOUT",
                    // Instructions for test
                    "456",guidePasswordInput,"Enter,","2",visitorTicketCode,visitorTicketCode,"Start","4","GETMEOUT"
                    
                }
            };
            Program.World = world;
                // Run main to make a reservation as a visitor
            Program.Main();

            // Act
            Program.Main();
            // Assert
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("Visitor has already been added."));
            
            List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(((FakeWorld)Program.World).Files[TourTools.JsonFilePath]);
            Tour tourWithGuide = tours.Where(tour => tour.AssignedGuide.Password == guidePassword).FirstOrDefault();

            Assert.IsTrue(tourWithGuide.PresentVisitors.Count == 1);
        }

        [TestMethod]
        public void TestGuideStartTourTryAddingWrongCode()
        {
            // Arrange
            string guidePassword = "Guide1";
            string guidePasswordInput = "G,u,i,d,e,1,";
            string wrongCode = "1234567890";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"}
                },
                LinesToRead = new()
                {
                    "456",guidePasswordInput,"Enter,","2",wrongCode,"Start","4","GETMEOUT"
                    
                }
            };
            Program.World = world;

            // Act
            Program.Main();
            // Assert
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("Invalid ticket. Please try again or write 'Start' to begin the tour."));
            
            List<Tour> tours = JsonConvert.DeserializeObject<List<Tour>>(((FakeWorld)Program.World).Files[TourTools.JsonFilePath]);
            Tour tourWithGuide = tours.Where(tour => tour.AssignedGuide.Password == guidePassword).FirstOrDefault();

            Assert.IsTrue(tourWithGuide.PresentVisitors.Count == 0);
        }
    }
}