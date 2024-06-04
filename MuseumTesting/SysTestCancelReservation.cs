using ReservationSystem;
using Newtonsoft.Json;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestVisitorCancelReservation
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
                    {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"},
                    {"./JSON-Files/Tours-20240601.json","[{\"TourId\":\"a90827ac-18d8-4569-807c-6a1e9fbbaa3f\",\"StartTime\":\"2024-06-01T11:00:00\",\"EndTime\":\"2024-06-01T11:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[{\"VisitorId\":\"fad423e0-ebcb-4e38-88f1-9320bb0d59df\",\"TicketCode\":\"1234567890\"}],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"bd01b437-9e6e-46eb-98fb-0b68f1244dbf\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"ae414d5b-bd4d-4e44-ad99-8d9761d8180c\",\"StartTime\":\"2024-06-01T11:15:00\",\"EndTime\":\"2024-06-01T11:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"784fe522-e541-4c35-a333-f13c302958fa\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"3534b225-6650-41f6-b7dd-44d53345e632\",\"StartTime\":\"2024-06-01T11:30:00\",\"EndTime\":\"2024-06-01T11:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"e6880c36-676c-4c0d-8f24-722ef7497252\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"454abc20-365f-42fb-8c40-0b32c68407fe\",\"StartTime\":\"2024-06-01T11:45:00\",\"EndTime\":\"2024-06-01T12:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"a33c4ad1-92d9-44bb-a409-5901a81208e0\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"d0f92bae-d4dd-4c3f-9b4f-11cc304822e6\",\"StartTime\":\"2024-06-01T12:00:00\",\"EndTime\":\"2024-06-01T12:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"c1bdc72f-1b36-4549-a6cc-c86a4b7182f6\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"9a2a647c-d92b-4cca-af97-fb1ade48f714\",\"StartTime\":\"2024-06-01T12:15:00\",\"EndTime\":\"2024-06-01T12:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"bd01b437-9e6e-46eb-98fb-0b68f1244dbf\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"fc92f631-a33f-45fd-9dad-c9dfe4f2fb20\",\"StartTime\":\"2024-06-01T12:30:00\",\"EndTime\":\"2024-06-01T12:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"784fe522-e541-4c35-a333-f13c302958fa\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"94365cd6-6633-43a3-832a-e6f35835ed6e\",\"StartTime\":\"2024-06-01T12:45:00\",\"EndTime\":\"2024-06-01T13:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"e6880c36-676c-4c0d-8f24-722ef7497252\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"5e80503a-b3ab-4526-bd39-92d23e816668\",\"StartTime\":\"2024-06-01T13:00:00\",\"EndTime\":\"2024-06-01T13:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"a33c4ad1-92d9-44bb-a409-5901a81208e0\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"5dbc1aa1-d1a4-4410-a43b-32f4a1a539bb\",\"StartTime\":\"2024-06-01T13:15:00\",\"EndTime\":\"2024-06-01T13:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"c1bdc72f-1b36-4549-a6cc-c86a4b7182f6\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"df703535-4172-469d-a41e-ec676be98e05\",\"StartTime\":\"2024-06-01T13:30:00\",\"EndTime\":\"2024-06-01T13:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"bd01b437-9e6e-46eb-98fb-0b68f1244dbf\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"88cb4ff8-42a8-4d53-af9e-f45b904f0b36\",\"StartTime\":\"2024-06-01T13:45:00\",\"EndTime\":\"2024-06-01T14:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"784fe522-e541-4c35-a333-f13c302958fa\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"5522e66d-e1ca-4c68-ad8c-740a54274879\",\"StartTime\":\"2024-06-01T14:00:00\",\"EndTime\":\"2024-06-01T14:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"e6880c36-676c-4c0d-8f24-722ef7497252\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"583bdcdb-4a75-4d94-8087-277751625b01\",\"StartTime\":\"2024-06-01T14:15:00\",\"EndTime\":\"2024-06-01T14:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"a33c4ad1-92d9-44bb-a409-5901a81208e0\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"2ab7d27c-3922-4520-9d4b-5c0b0c5f38f0\",\"StartTime\":\"2024-06-01T14:30:00\",\"EndTime\":\"2024-06-01T14:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"c1bdc72f-1b36-4549-a6cc-c86a4b7182f6\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"ef5f5f65-496d-4c0a-b26c-1b56d77dda51\",\"StartTime\":\"2024-06-01T14:45:00\",\"EndTime\":\"2024-06-01T15:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"bd01b437-9e6e-46eb-98fb-0b68f1244dbf\",\"Name\":\"John Doe\",\"Password\":null}},{\"TourId\":\"1335133d-7cc1-4193-ad9b-90f234da8c5e\",\"StartTime\":\"2024-06-01T15:00:00\",\"EndTime\":\"2024-06-01T15:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"784fe522-e541-4c35-a333-f13c302958fa\",\"Name\":\"Alice Johnson\",\"Password\":null}},{\"TourId\":\"4f97115d-9f48-4237-9425-4ddec120271f\",\"StartTime\":\"2024-06-01T15:15:00\",\"EndTime\":\"2024-06-01T15:30:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"e6880c36-676c-4c0d-8f24-722ef7497252\",\"Name\":\"Steve Brown\",\"Password\":null}},{\"TourId\":\"d4516621-c21a-46d5-9c1f-9420e05dcc4a\",\"StartTime\":\"2024-06-01T15:30:00\",\"EndTime\":\"2024-06-01T15:45:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"a33c4ad1-92d9-44bb-a409-5901a81208e0\",\"Name\":\"Mary Lee\",\"Password\":null}},{\"TourId\":\"cd07fc89-4914-4c7f-96a3-fbf38024ed1d\",\"StartTime\":\"2024-06-01T15:45:00\",\"EndTime\":\"2024-06-01T16:00:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"c1bdc72f-1b36-4549-a6cc-c86a4b7182f6\",\"Name\":\"Tom Clark\",\"Password\":null}},{\"TourId\":\"317b101b-0944-4a9a-a89a-177518b1939a\",\"StartTime\":\"2024-06-01T16:00:00\",\"EndTime\":\"2024-06-01T16:15:00\",\"Duration\":15,\"MaxCapacity\":13,\"ExpectedVisitors\":[],\"PresentVisitors\":[],\"Started\":false,\"Deleted\":false,\"AssignedGuide\":{\"GuideId\":\"bd01b437-9e6e-46eb-98fb-0b68f1244dbf\",\"Name\":\"John Doe\",\"Password\":null}}]"}
                },
                LinesToRead = new()
                {
                    visitorTicketCode,"2","Enter,","GETMEOUT"
                }
            };
            Program.World = world;

            // Act
            Program.Main();

            // Assert
            // position of the string where the cancel message starts
            int positionCancelMessage = world.LinesWritten.FindIndex(s => s == "Your reservation has been ");
            string actualCancelMessage = world.LinesWritten[positionCancelMessage] + world.LinesWritten[positionCancelMessage + 1] + world.LinesWritten[positionCancelMessage + 2];

            Assert.AreEqual("Your reservation has been cancelled  successfully\n", actualCancelMessage);

            Tour tourAfterCancel = TourTools.FindTourByVisitorTicketCode(visitorTicketCode);
            Assert.IsTrue(tourAfterCancel == null);
        }
    }
}