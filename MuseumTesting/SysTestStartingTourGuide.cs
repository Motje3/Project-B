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
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"99df0a07-a667-4875-82e7-213371ffbee2\",\"StartTime\":\"2024-06-01T09:00:00\",\"EndTime\":\"2024-06-01T09:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"873d138e-3720-4a89-be7b-e2f0ae0172cb\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"54feb388-e819-489f-9e80-d4c8c13d9d72\",\"StartTime\":\"2024-06-01T09:20:00\",\"EndTime\":\"2024-06-01T09:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"81c380e0-8531-41e2-9e0b-866eeec30be1\",\"StartTime\":\"2024-06-01T09:40:00\",\"EndTime\":\"2024-06-01T10:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"409c95d8-a317-4b6a-85f7-5cffdf7245be\",\"StartTime\":\"2024-06-01T10:00:00\",\"EndTime\":\"2024-06-01T10:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"6951265d-dd36-431b-a2ac-53fc53ecd874\",\"StartTime\":\"2024-06-01T10:20:00\",\"EndTime\":\"2024-06-01T10:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"1f3c2c27-78c8-48b0-9e35-fa45c3b494a3\",\"StartTime\":\"2024-06-01T10:40:00\",\"EndTime\":\"2024-06-01T11:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"90d62a3a-7d29-4953-a21d-8d26ccb3e2d1\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"31237530-76d7-446e-b955-6e71d3698ddc\",\"StartTime\":\"2024-06-01T11:20:00\",\"EndTime\":\"2024-06-01T11:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"551dc3e1-2385-4c07-acd4-781be5a70739\",\"StartTime\":\"2024-06-01T11:40:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"b0695036-09da-494e-afab-0d545b724d69\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"377c087e-6705-472d-b1fc-7a19b5e4b3ac\",\"StartTime\":\"2024-06-01T12:20:00\",\"EndTime\":\"2024-06-01T12:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"4ec570fb-dce7-4c18-bb7b-5f9310cbc45c\",\"StartTime\":\"2024-06-01T12:40:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"a2b886d0-ecd9-430e-a7fc-de545ff8947c\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"63270e20-136c-4e20-82b8-ddc65107411b\",\"StartTime\":\"2024-06-01T13:20:00\",\"EndTime\":\"2024-06-01T13:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"5efb992a-7ec7-4af3-9540-69312a3bc762\",\"StartTime\":\"2024-06-01T13:40:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"fe2e9969-1bf3-42ff-9578-072c195c729f\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"27cf4d00-ea43-4837-ad38-1fc86c14d02c\",\"StartTime\":\"2024-06-01T14:20:00\",\"EndTime\":\"2024-06-01T14:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"a9d6b73d-f517-4d7e-b5aa-101c35e5710b\",\"StartTime\":\"2024-06-01T14:40:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"3942bf3b-fc76-453e-bfcf-098a2db7ae5b\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"098f4dac-e9f0-419c-9420-2394a1289e86\",\"StartTime\":\"2024-06-01T15:20:00\",\"EndTime\":\"2024-06-01T15:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"9cfb7c15-45a5-471d-bab4-37b924ca0b9e\",\"StartTime\":\"2024-06-01T15:40:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"6d94e5ee-1899-419b-b1e0-248e7861d853\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}}]"}
                },
                LinesToRead = new()
                {
                    "456",guidePasswordInput,"Enter,","2",visitorTicketCode,"Start","4","GETMEOUT"
                }
            };
            Program.World = world;

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
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]},{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"GuideName\":\"Alice Johnson\",\"Password\":\"22\",\"Tours\":[{\"StartTime\":\"09:20 AM\"},{\"StartTime\":\"10:20 AM\"},{\"StartTime\":\"11:20 AM\"},{\"StartTime\":\"12:20 PM\"},{\"StartTime\":\"01:20 PM\"},{\"StartTime\":\"02:20 PM\"},{\"StartTime\":\"03:20 PM\"}]},{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"GuideName\":\"Steve Brown\",\"Password\":\"33\",\"Tours\":[{\"StartTime\":\"09:40 AM\"},{\"StartTime\":\"10:40 AM\"},{\"StartTime\":\"11:40 AM\"},{\"StartTime\":\"12:40 PM\"},{\"StartTime\":\"01:40 PM\"},{\"StartTime\":\"02:40 PM\"},{\"StartTime\":\"03:40 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"e8137f69-5401-49d1-9b86-c31ae853e816\",\"StartTime\":\"2024-06-01T09:00:00\",\"EndTime\":\"2024-06-01T09:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"85747b72-1e2a-4c31-b1d4-2d2b9096658a\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"a8df589d-36fe-40cd-93da-0ed45900fca4\",\"StartTime\":\"2024-06-01T09:20:00\",\"EndTime\":\"2024-06-01T09:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"2be089c7-b25b-417e-bee2-cb477433c8f5\",\"StartTime\":\"2024-06-01T09:40:00\",\"EndTime\":\"2024-06-01T10:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"e4e6fb47-9b76-4365-b943-d7cb36860a99\",\"StartTime\":\"2024-06-01T10:00:00\",\"EndTime\":\"2024-06-01T10:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"9fee2e4e-632a-410d-80e2-fe8813e4e1c3\",\"StartTime\":\"2024-06-01T10:20:00\",\"EndTime\":\"2024-06-01T10:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"7d9f5d1d-2f2b-4195-8d34-1e6288f19920\",\"StartTime\":\"2024-06-01T10:40:00\",\"EndTime\":\"2024-06-01T11:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"b92b5a3b-95b5-482d-b6c2-9f59dbf3fa63\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"01dc05b7-e07e-4142-b515-26664fbca0b5\",\"StartTime\":\"2024-06-01T11:20:00\",\"EndTime\":\"2024-06-01T11:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"f0b10b8e-b6b9-467a-a9db-be2aef17d0db\",\"StartTime\":\"2024-06-01T11:40:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"04ff5832-6f41-4bc6-b737-0279893c1562\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"343fbbb2-52dd-44aa-8c78-b41792a2d8cc\",\"StartTime\":\"2024-06-01T12:20:00\",\"EndTime\":\"2024-06-01T12:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"9cd7eddf-3818-44a2-88c3-caa47f8d48c2\",\"StartTime\":\"2024-06-01T12:40:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"102223a1-414d-4e50-8f9b-fece65966e2b\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"3ba88b81-300c-4f56-80df-b174e7baf902\",\"StartTime\":\"2024-06-01T13:20:00\",\"EndTime\":\"2024-06-01T13:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"d6277689-14bf-44b3-9b42-d9515585b624\",\"StartTime\":\"2024-06-01T13:40:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"093558e2-f102-4cb0-8ff9-f2879664a822\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"d301e659-ac1e-4337-a0ca-a9b31fae0907\",\"StartTime\":\"2024-06-01T14:20:00\",\"EndTime\":\"2024-06-01T14:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"5031b4ae-24eb-4b6d-83ea-627d3343fd4e\",\"StartTime\":\"2024-06-01T14:40:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"50e324bf-2baf-4df3-bb2e-832424e38055\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"408d9aad-f397-4966-89b5-8595481c4293\",\"StartTime\":\"2024-06-01T15:20:00\",\"EndTime\":\"2024-06-01T15:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"466c8ebb-c782-439c-af1b-eee6e7d11a31\",\"StartTime\":\"2024-06-01T15:40:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"589f43ca-0e7a-42dd-949c-435afe5d7b59\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}}]"}
                },
                LinesToRead = new()
                {
                    "456",guidePasswordInput,"Enter,","2",visitorTicketCode,visitorTicketCode,"Start","4","GETMEOUT"
                }
            };
            Program.World = world;

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