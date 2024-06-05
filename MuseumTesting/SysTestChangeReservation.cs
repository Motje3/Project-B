using ReservationSystem;
using Newtonsoft.Json;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestVisitorChangeReservation
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
        public void TestVisitorChangeReservationMenuSucceful()
        {
            // Arrange
            string tourChoice = "1";
            string newTourChoice = "2";
            string visitorTicketCode = "1234567890";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", "[\"1234567890\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"11:00:00\",\"EndTime\": \"16:15:00\",\"Duration\": 15,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"d9c6a9da-6e9c-4b2f-bd20-14386b91ec00\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"8fa0821c-c821-4ff0-914d-ba69e694a391\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d3f383d6-9c3f-4708-b604-45aee49c0751\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"0b9153f0-8dbb-43b8-95f6-9e69ffd7aa1e\",\"StartTime\":\"2024-06-01T11:15:00\",\"EndTime\":\"2024-06-01T11:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"beaddd1f-69fb-4e55-afa6-925a8995abd7\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"87e07d81-b1e2-4326-a2d5-aafbeef49bcf\",\"StartTime\":\"2024-06-01T11:30:00\",\"EndTime\":\"2024-06-01T11:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"42dca162-5bee-4a52-aa68-86b35e6e1fa8\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"d9e46229-c704-4e3d-926c-0a47f240dccf\",\"StartTime\":\"2024-06-01T11:45:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d94b10b4-d72a-49e1-9a72-98a43d935afb\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"4be23818-8806-42d4-9000-fd60569ee612\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"07e19ae5-64df-497a-ae1d-12d606376354\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"71be6ee9-64fe-48cb-aeb2-d99add680967\",\"StartTime\":\"2024-06-01T12:15:00\",\"EndTime\":\"2024-06-01T12:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d3f383d6-9c3f-4708-b604-45aee49c0751\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"23caf24b-cf98-4412-9cb0-9706853a4141\",\"StartTime\":\"2024-06-01T12:30:00\",\"EndTime\":\"2024-06-01T12:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"beaddd1f-69fb-4e55-afa6-925a8995abd7\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"3b6bd50a-3987-4e6d-97b9-d06f85c062d7\",\"StartTime\":\"2024-06-01T12:45:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"42dca162-5bee-4a52-aa68-86b35e6e1fa8\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"73d642e7-cc11-4539-96d7-1a382df81211\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d94b10b4-d72a-49e1-9a72-98a43d935afb\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"7b2cabf2-dda8-4ee2-88ba-41076147d677\",\"StartTime\":\"2024-06-01T13:15:00\",\"EndTime\":\"2024-06-01T13:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"07e19ae5-64df-497a-ae1d-12d606376354\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"be64d9f7-c2c4-4dcb-89dd-863defdc9c7e\",\"StartTime\":\"2024-06-01T13:30:00\",\"EndTime\":\"2024-06-01T13:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d3f383d6-9c3f-4708-b604-45aee49c0751\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"5c655fc4-6851-4ea4-926e-59e9fd3bf8e7\",\"StartTime\":\"2024-06-01T13:45:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"beaddd1f-69fb-4e55-afa6-925a8995abd7\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"c00fdf3b-7a28-4e7d-87bb-ed88c033118e\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"42dca162-5bee-4a52-aa68-86b35e6e1fa8\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"9264de31-2d2a-436a-a74b-85c920b82197\",\"StartTime\":\"2024-06-01T14:15:00\",\"EndTime\":\"2024-06-01T14:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d94b10b4-d72a-49e1-9a72-98a43d935afb\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"e79ef42f-7a41-4818-aa91-e05238d4a048\",\"StartTime\":\"2024-06-01T14:30:00\",\"EndTime\":\"2024-06-01T14:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"07e19ae5-64df-497a-ae1d-12d606376354\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"f08b7e1c-bea0-4afa-9e66-a07277eb79b9\",\"StartTime\":\"2024-06-01T14:45:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d3f383d6-9c3f-4708-b604-45aee49c0751\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"a43a8b18-5fde-46fa-814f-78c6754f931b\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"beaddd1f-69fb-4e55-afa6-925a8995abd7\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"403dc65c-8b9e-4a8f-b1f6-558fd4add2bd\",\"StartTime\":\"2024-06-01T15:15:00\",\"EndTime\":\"2024-06-01T15:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"42dca162-5bee-4a52-aa68-86b35e6e1fa8\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"e9056c0a-9102-4ef4-8217-a3ddb8da37b2\",\"StartTime\":\"2024-06-01T15:30:00\",\"EndTime\":\"2024-06-01T15:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d94b10b4-d72a-49e1-9a72-98a43d935afb\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"20bb94b4-06c9-406d-9817-e0fcdf793f04\",\"StartTime\":\"2024-06-01T15:45:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"07e19ae5-64df-497a-ae1d-12d606376354\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"25595e7e-0022-47a9-8bf9-914b0f46ad5f\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"d3f383d6-9c3f-4708-b604-45aee49c0751\",\"Name\":\"John Doe\",\"Password\":null}}]"}
                },
                LinesToRead = new()
                {
                    visitorTicketCode,tourChoice,newTourChoice,"Enter,","GETMEOUT"
                }
            };
            Program.World = world;

            // Act
            List<Tour> toursBeforeTransfer = JsonConvert.DeserializeObject<List<Tour>>(world.Files["./JSON-Files/Tours-20240601.json"]);
            Tour tourBeforeTransfer = toursBeforeTransfer.Where(t => t.ExpectedVisitors.Select(v => v.TicketCode).ToList().Contains(visitorTicketCode)).FirstOrDefault();

            Program.Main();

            // Assert
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("\nYou have successfully transferred to the new tour at "));

            Tour tourAfterTransfer = TourTools.FindTourByVisitorTicketCode(visitorTicketCode);
            Assert.IsTrue(tourAfterTransfer.StartTime != tourBeforeTransfer.StartTime);
        }

        [TestMethod]
        public void TestVisitorChangeReservationMenuWrongInput()
        {
            // Arrange
            string tourChoice = "1";
            string newTourChoice = "XXX";
            string visitorTicketCode = "1234567890";
            FakeWorld world = new()
            {
                Today = new DateTime(2024, 6, 1),
                Now = new DateTime(2024, 6, 1),
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", "[\"1234567890\"]"},
                    {"./JSON-Files/TourSettings.json","{\"StartTime\": \"11:00:00\",\"EndTime\": \"16:15:00\",\"Duration\": 15,\"MaxCapacity\": 13}"},
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"de724571-33ae-4910-8ba4-088a11a1e849\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"0ad9292b-c49d-445d-87fc-f2abb06e56f2\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"f576f39b-8da9-475f-8e04-d9a371564a9c\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"91caee20-9b0c-4eeb-a907-f4181d1b1034\",\"StartTime\":\"2024-06-01T11:15:00\",\"EndTime\":\"2024-06-01T11:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"8107804a-fa15-47ac-974c-061f72441de6\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"8966b693-5e69-48d6-b690-d59ef76ec15a\",\"StartTime\":\"2024-06-01T11:30:00\",\"EndTime\":\"2024-06-01T11:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"9d86c203-22fb-4bbf-a1ba-db80e67c3d64\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"46d86af7-4813-423b-824b-a02e52ba162e\",\"StartTime\":\"2024-06-01T11:45:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"deffdfc1-c0dc-461c-835d-2d473fcab58a\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"73114562-b520-4c38-8db8-a170a760fde1\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"ed8cee34-4f17-4782-92ed-23bfc381130e\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"4f3bc250-39ae-40c2-9fc8-8bdd5ac439c7\",\"StartTime\":\"2024-06-01T12:15:00\",\"EndTime\":\"2024-06-01T12:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"f576f39b-8da9-475f-8e04-d9a371564a9c\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"d8030b5a-3456-45ed-b041-b6f700189931\",\"StartTime\":\"2024-06-01T12:30:00\",\"EndTime\":\"2024-06-01T12:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"8107804a-fa15-47ac-974c-061f72441de6\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"37306dbd-2491-46a0-b990-d9bb4b13beb4\",\"StartTime\":\"2024-06-01T12:45:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"9d86c203-22fb-4bbf-a1ba-db80e67c3d64\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"f1a8f851-1255-4600-a01d-7b9ba103598e\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"deffdfc1-c0dc-461c-835d-2d473fcab58a\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"93ba56c7-de02-40d4-a38c-1f9a8eb08811\",\"StartTime\":\"2024-06-01T13:15:00\",\"EndTime\":\"2024-06-01T13:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"ed8cee34-4f17-4782-92ed-23bfc381130e\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"90c71175-5ff2-4315-a205-5782d2c2b6c5\",\"StartTime\":\"2024-06-01T13:30:00\",\"EndTime\":\"2024-06-01T13:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"f576f39b-8da9-475f-8e04-d9a371564a9c\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"68318326-9097-4c19-8b84-beb6b5682cbd\",\"StartTime\":\"2024-06-01T13:45:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"8107804a-fa15-47ac-974c-061f72441de6\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"b6fb6e9f-851b-40c4-b543-54b2bc13e42a\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"9d86c203-22fb-4bbf-a1ba-db80e67c3d64\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"7948bd10-78e7-467a-8033-419d526ec015\",\"StartTime\":\"2024-06-01T14:15:00\",\"EndTime\":\"2024-06-01T14:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"deffdfc1-c0dc-461c-835d-2d473fcab58a\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"8ce654c8-ee92-48f0-a3ae-b1e3b4208981\",\"StartTime\":\"2024-06-01T14:30:00\",\"EndTime\":\"2024-06-01T14:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"ed8cee34-4f17-4782-92ed-23bfc381130e\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"1efa868d-ca60-4828-b68a-8448d6e6d44e\",\"StartTime\":\"2024-06-01T14:45:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"f576f39b-8da9-475f-8e04-d9a371564a9c\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"d05440ac-54be-4f58-b0e9-8ec328c59628\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"8107804a-fa15-47ac-974c-061f72441de6\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"3c73190d-c816-4ce3-9341-bf692dbafe33\",\"StartTime\":\"2024-06-01T15:15:00\",\"EndTime\":\"2024-06-01T15:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"9d86c203-22fb-4bbf-a1ba-db80e67c3d64\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"1ebed77f-17f0-483b-a17f-fc3e5fcb484b\",\"StartTime\":\"2024-06-01T15:30:00\",\"EndTime\":\"2024-06-01T15:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"deffdfc1-c0dc-461c-835d-2d473fcab58a\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"b71d53df-f8da-46af-965d-af125549e83a\",\"StartTime\":\"2024-06-01T15:45:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"ed8cee34-4f17-4782-92ed-23bfc381130e\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"67b17c08-61af-4acf-bcdc-5899097bf4ca\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"f576f39b-8da9-475f-8e04-d9a371564a9c\",\"Name\":\"John Doe\",\"Password\":null}}]"}
                },
                LinesToRead = new()
                {
                    visitorTicketCode,"1",newTourChoice,"GETMEOUT","3","GETMEOUT"
                }
            };
            Program.World = world;

            List<Tour> toursBeforeTransfer = JsonConvert.DeserializeObject<List<Tour>>(world.Files["./JSON-Files/Tours-20240601.json"]);
            Tour tourBeforeTransfer = toursBeforeTransfer.Where(t => t.ExpectedVisitors.Select(v => v.TicketCode).ToList().Contains(visitorTicketCode)).FirstOrDefault();

            // Act
            Program.Main();

            // Assert
            Assert.IsTrue(((FakeWorld)Program.World).LinesWritten.Contains("*    Invalid choice    *"));

            Tour tourAfterTransfer = TourTools.FindTourByVisitorTicketCode(visitorTicketCode);
            Assert.IsTrue(tourBeforeTransfer.StartTime == tourAfterTransfer.StartTime);
        }
    }
}