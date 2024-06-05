using System.Globalization;
using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class GuideTests
    {
        private const string GuideAssignmentsFilePath = "./JSON-Files/GuideAssignments.json";
        [TestInitialize]
        public void setUp()
        {
            Guide.AllGuides.Clear();
        }

        // THIS TEST DOES NOT MAKE SENSE BECAUSE YOU DONT ADD A TOUR TO ANYTHING OR DO ANYTHING WITH tourId
        /*
        [TestMethod]
        public void Constructor_WithTourId_AddsTourToAssignedTourIds()
        {
            
            // Arrange
            var guidid = Guid.NewGuid();
            Guid tourId = Guid.NewGuid();

            // Act
            var guide = new Guide(guidid, "John", "111");

            // Assert
            Assert.IsTrue(guide.AssignedTourIds.Contains(tourId));
        }
        */

        [TestMethod]
        public void AssignTour_WithNewTour_AddsTourToAssignedTourIds()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var guide = new Guide(Guid.NewGuid(), "John", "111");


            // Act
            guide.AssignTour(tourId);

            // Assert
            Assert.IsTrue(guide.AssignedTourIds.Contains(tourId));
        }

        [TestMethod]
        public void LoadGuides_WithValidGuideAssignments_LoadsGuides()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/GuideAssignments.json", "[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\"1111\",\"Tours\":[]},{\"GuideId\":\"6c8cb11c-cf42-4606-8778-283155c5fda8\",\"GuideName\":\"Jane Doe\",\"Password\":\"4444\",\"Tours\":[]}]"}
                }
            };
            Program.World = world;

            // Act
            Guide.LoadGuides();

            // Assert
            Assert.AreEqual(2, Guide.AllGuides.Count);
            Assert.IsTrue(Guide.AllGuides.Any(g => g.Name == "John Doe"));
            Assert.IsTrue(Guide.AllGuides.Any(g => g.Name == "Jane Doe"));

        }

        [TestMethod]
        public void AddVisitorLastMinuteTest()
        {
            // Arrange
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"1111\",\"2222\",\"3333\",\"5555\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\"111\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Jackson\",\"Password\":\"222\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"99df0a07-a667-4875-82e7-213371ffbee2\",\"StartTime\":\"2024-06-01T09:00:00\",\"EndTime\":\"2024-06-01T09:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"54feb388-e819-489f-9e80-d4c8c13d9d72\",\"StartTime\":\"2024-06-01T09:20:00\",\"EndTime\":\"2024-06-01T09:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"81c380e0-8531-41e2-9e0b-866eeec30be1\",\"StartTime\":\"2024-06-01T09:40:00\",\"EndTime\":\"2024-06-01T10:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"409c95d8-a317-4b6a-85f7-5cffdf7245be\",\"StartTime\":\"2024-06-01T10:00:00\",\"EndTime\":\"2024-06-01T10:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"6951265d-dd36-431b-a2ac-53fc53ecd874\",\"StartTime\":\"2024-06-01T10:20:00\",\"EndTime\":\"2024-06-01T10:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"1f3c2c27-78c8-48b0-9e35-fa45c3b494a3\",\"StartTime\":\"2024-06-01T10:40:00\",\"EndTime\":\"2024-06-01T11:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"90d62a3a-7d29-4953-a21d-8d26ccb3e2d1\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"31237530-76d7-446e-b955-6e71d3698ddc\",\"StartTime\":\"2024-06-01T11:20:00\",\"EndTime\":\"2024-06-01T11:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"551dc3e1-2385-4c07-acd4-781be5a70739\",\"StartTime\":\"2024-06-01T11:40:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"b0695036-09da-494e-afab-0d545b724d69\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"377c087e-6705-472d-b1fc-7a19b5e4b3ac\",\"StartTime\":\"2024-06-01T12:20:00\",\"EndTime\":\"2024-06-01T12:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"4ec570fb-dce7-4c18-bb7b-5f9310cbc45c\",\"StartTime\":\"2024-06-01T12:40:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"a2b886d0-ecd9-430e-a7fc-de545ff8947c\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"63270e20-136c-4e20-82b8-ddc65107411b\",\"StartTime\":\"2024-06-01T13:20:00\",\"EndTime\":\"2024-06-01T13:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"5efb992a-7ec7-4af3-9540-69312a3bc762\",\"StartTime\":\"2024-06-01T13:40:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"fe2e9969-1bf3-42ff-9578-072c195c729f\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"27cf4d00-ea43-4837-ad38-1fc86c14d02c\",\"StartTime\":\"2024-06-01T14:20:00\",\"EndTime\":\"2024-06-01T14:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"a9d6b73d-f517-4d7e-b5aa-101c35e5710b\",\"StartTime\":\"2024-06-01T14:40:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"3942bf3b-fc76-453e-bfcf-098a2db7ae5b\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}},{\"TourId\":\"098f4dac-e9f0-419c-9420-2394a1289e86\",\"StartTime\":\"2024-06-01T15:20:00\",\"EndTime\":\"2024-06-01T15:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Jackson\",\"Password\":\"222\"}},{\"TourId\":\"9cfb7c15-45a5-471d-bab4-37b924ca0b9e\",\"StartTime\":\"2024-06-01T15:40:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"6d94e5ee-1899-419b-b1e0-248e7861d853\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"111\"}}]"}
                }
            };
            Program.World = world;


            Visitor Bob = new Visitor("2222");
            Visitor Mark = new Visitor("3333");
            Visitor Carl = new Visitor("5555");

            Guide.LoadGuides();
            TourTools.InitializeTours();

            string JohnPassword = "111";
            Guide John = Guide.AllGuides.Where(g => g.Password == JohnPassword).FirstOrDefault();
            string AlicePassword = "222";
            Guide Alice = Guide.AllGuides.Where(g => g.Password == AlicePassword).FirstOrDefault();

            // Act
            John.AddVisitorLastMinute(Bob);
            John.AddVisitorLastMinute(Mark);
            Alice.AddVisitorLastMinute(Carl);

            // Add tours to today's tours

            // Assert
            // John should have 2 presentvistors (tour1)
            // Alice should have 1 presentvistors (tour2)
            Tour tourWithJohn = TourTools.TodaysTours.Where(t => t.AssignedGuide.Password == JohnPassword).FirstOrDefault();
            Assert.AreEqual(2, tourWithJohn.PresentVisitors.Count);
            Tour tourWithAlice = TourTools.TodaysTours.Where(t => t.AssignedGuide.Password == AlicePassword).FirstOrDefault();
            Assert.AreEqual(1, tourWithAlice.PresentVisitors.Count);
        }
    }
}


