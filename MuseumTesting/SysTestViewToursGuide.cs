using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestViewToursGuide
    {
        [TestCleanup]
        public void CleanUp()
        {
            TourTools.TodaysTours.Clear();
        }

        [TestMethod]
        public void TestGuideViewTours()
        {
            // Arrange
            string guidePassword = "Guide1";
            string guidePasswordInput = "G,u,i,d,e,1,";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[{\"StartTime\":\"09:00 AM\"},{\"StartTime\":\"10:00 AM\"},{\"StartTime\":\"11:00 AM\"},{\"StartTime\":\"12:00 PM\"},{\"StartTime\":\"01:00 PM\"},{\"StartTime\":\"02:00 PM\"},{\"StartTime\":\"03:00 PM\"},{\"StartTime\":\"04:00 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"99df0a07-a667-4875-82e7-213371ffbee2\",\"StartTime\":\"2024-06-01T09:00:00\",\"EndTime\":\"2024-06-01T09:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"873d138e-3720-4a89-be7b-e2f0ae0172cb\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"54feb388-e819-489f-9e80-d4c8c13d9d72\",\"StartTime\":\"2024-06-01T09:20:00\",\"EndTime\":\"2024-06-01T09:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"81c380e0-8531-41e2-9e0b-866eeec30be1\",\"StartTime\":\"2024-06-01T09:40:00\",\"EndTime\":\"2024-06-01T10:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"409c95d8-a317-4b6a-85f7-5cffdf7245be\",\"StartTime\":\"2024-06-01T10:00:00\",\"EndTime\":\"2024-06-01T10:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"6951265d-dd36-431b-a2ac-53fc53ecd874\",\"StartTime\":\"2024-06-01T10:20:00\",\"EndTime\":\"2024-06-01T10:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"1f3c2c27-78c8-48b0-9e35-fa45c3b494a3\",\"StartTime\":\"2024-06-01T10:40:00\",\"EndTime\":\"2024-06-01T11:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"90d62a3a-7d29-4953-a21d-8d26ccb3e2d1\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"31237530-76d7-446e-b955-6e71d3698ddc\",\"StartTime\":\"2024-06-01T11:20:00\",\"EndTime\":\"2024-06-01T11:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"551dc3e1-2385-4c07-acd4-781be5a70739\",\"StartTime\":\"2024-06-01T11:40:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"b0695036-09da-494e-afab-0d545b724d69\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"377c087e-6705-472d-b1fc-7a19b5e4b3ac\",\"StartTime\":\"2024-06-01T12:20:00\",\"EndTime\":\"2024-06-01T12:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"4ec570fb-dce7-4c18-bb7b-5f9310cbc45c\",\"StartTime\":\"2024-06-01T12:40:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"a2b886d0-ecd9-430e-a7fc-de545ff8947c\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"63270e20-136c-4e20-82b8-ddc65107411b\",\"StartTime\":\"2024-06-01T13:20:00\",\"EndTime\":\"2024-06-01T13:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"5efb992a-7ec7-4af3-9540-69312a3bc762\",\"StartTime\":\"2024-06-01T13:40:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"fe2e9969-1bf3-42ff-9578-072c195c729f\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"27cf4d00-ea43-4837-ad38-1fc86c14d02c\",\"StartTime\":\"2024-06-01T14:20:00\",\"EndTime\":\"2024-06-01T14:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"a9d6b73d-f517-4d7e-b5aa-101c35e5710b\",\"StartTime\":\"2024-06-01T14:40:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"3942bf3b-fc76-453e-bfcf-098a2db7ae5b\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}},{\"TourId\":\"098f4dac-e9f0-419c-9420-2394a1289e86\",\"StartTime\":\"2024-06-01T15:20:00\",\"EndTime\":\"2024-06-01T15:40:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"2d739229-6220-4d32-84c1-c18a812f2774\",\"Name\":\"Alice Johnson\",\"Password\":\"22\"}},{\"TourId\":\"9cfb7c15-45a5-471d-bab4-37b924ca0b9e\",\"StartTime\":\"2024-06-01T15:40:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"1cae7c8d-bec3-416d-a4a3-3023ff0bc402\",\"Name\":\"Steve Brown\",\"Password\":\"33\"}},{\"TourId\":\"6d94e5ee-1899-419b-b1e0-248e7861d853\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:20:00\",\"Duration\":20,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"Name\":\"John Doe\",\"Password\":\"Guide1\"}}]"}
                },
                LinesToRead = new()
                {
                    "1122334455",guidePasswordInput,"Enter,","1","4","GETMEOUT"
                }
            };
            Program.World = world;

            // Act
            Program.Main();

            // Assert
            List<Tour> toursAfterTest = JsonConvert.DeserializeObject<List<Tour>>(((FakeWorld)Program.World).Files[TourTools.JsonFilePath]);
            List<AdminBackEnd.GuideAssignment> guideAssignments = JsonConvert.DeserializeObject<List<AdminBackEnd.GuideAssignment>>(Program.World.ReadAllText(TourTools.JsonGuideAssignmentsPath));
            var amountActualAssignments = guideAssignments[0].Tours.Count;
            int indexToursForGuideStart = world.LinesWritten.FindIndex(s => s == $"Tours for {guideAssignments[0].GuideName}:\n");
            // Test each printed line with tours
            for (int lineIndex = 1; lineIndex <= amountActualAssignments; lineIndex++)
            {
                string[] currentLine = world.LinesWritten[indexToursForGuideStart + lineIndex].Split(" | ");
                Assert.IsTrue(currentLine[0].Contains("Tour "));
                Assert.IsTrue(currentLine[1].Contains($"Date: {DateOnly.FromDateTime(world.Today)}"));
                string ParsedTime = $"{TimeOnly.Parse(guideAssignments[0].Tours[lineIndex - 1].StartTime)}";
                Assert.IsTrue(currentLine[2].Contains($"Start Time: {ParsedTime}"));
                Assert.IsTrue(currentLine[3].Contains($"{toursAfterTest[lineIndex - 1].ExpectedVisitors.Count} reservations"));
            }
            // Test that line after last printed line with tours is the one expected
            Assert.IsTrue(world.LinesWritten[indexToursForGuideStart + amountActualAssignments + 1] == "\nWelcome ");
        }

        [TestMethod]
        public void TestGuideViewToursNoTours()
        {
            // Arrange
            string guidePassword = "Guide1";
            string guidePasswordInput = "G,u,i,d,e,1,";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"9:00:00\",\"EndTime\": \"16:40:00\",\"Duration\": 20,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+guidePassword+"\",\"Tours\":[]}]"},
                },
                LinesToRead = new()
                {
                    "1122334455",guidePasswordInput,"Enter,","1","4","GETMEOUT"
                }
            };
            Program.World = world;

            // Act
            Program.Main();

            // Assert 
            Assert.IsTrue(world.LinesWritten.Contains("You have no more tours today"));
        }
    }
}