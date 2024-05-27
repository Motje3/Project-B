using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class TourTests
    {
        private static string tourSettingsPath = "./JSON-Files/TourSettings.json";
        private static string guideAssignmentsPath = "./JSON-Files/GuideAssignments.json";
        private static string toursPath;

        [ClassInitialize]
        public static void ClassSetup(TestContext context)
        {
            // Ensure the directory exists
            Directory.CreateDirectory("./JSON-Files");

            // Mock settings and guide assignments for the day
            string settingsJson = JsonConvert.SerializeObject(new
            {
                StartTime = "09:00 AM",
                EndTime = "05:00 PM",
                Duration = 40, // Duration in minutes
                MaxCapacity = 13
            });
            File.WriteAllText(tourSettingsPath, settingsJson);

            string assignmentsJson = JsonConvert.SerializeObject(new List<dynamic>
            {
                new
                {
                    GuideName = "John Doe",
                    Tours = new dynamic[]
                    {
                        new { StartTime = "09:00 AM" },
                        new { StartTime = "10:00 AM" }
                    }
                }
            });
            File.WriteAllText(guideAssignmentsPath, assignmentsJson);

            // Set the path for today's tours
            toursPath = Tour.JsonFilePath;
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            if (File.Exists(tourSettingsPath)) File.Delete(tourSettingsPath);
            if (File.Exists(guideAssignmentsPath)) File.Delete(guideAssignmentsPath);
            if (File.Exists(toursPath)) File.Delete(toursPath);
        }

        [TestMethod]
        public void InitializeTours_WithExistingFile_LoadsTours()
        {
            // Arrange
            string filePath = Tour.JsonFilePath;
            // Prepare test data that matches the expected format
            string testData = "[{\"TourId\":\"" + Guid.NewGuid().ToString() + "\",\"StartTime\":\"" + DateTime.Now.ToString("s") + "\",\"Duration\":60,\"MaxCapacity\":30,\"Completed\":false,\"Deleted\":false,\"AssignedGuide\":{\"Name\":\"John Doe\"}}]";
            File.WriteAllText(filePath, testData);

            try
            {
                // Act
                Tour.InitializeTours();

                // Assert
                Assert.IsTrue(Tour.TodaysTours.Count > 0);
                Assert.AreEqual(30, Tour.TodaysTours[0].MaxCapacity);
            }
            finally
            {
                // Cleanup: Remove test data file
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        [TestMethod]
        public void AddVisitor_WithValidConditions_AddsVisitor()
        {
            // Setup
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 40, 13, false, false, new Guide(Guid.NewGuid(), "John", "111"));

            // Act
            tour.AddVisitor(visitor);

            // Assert
            Assert.IsTrue(tour.ExpectedVisitors.Contains(visitor));
        }

        [TestMethod]
        public void RemoveVisitor_RemovesExpectedVisitor()
        {
            // Setup
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 40, 13, false, false, new Guide(Guid.NewGuid(), "John", "111"));
            tour.AddVisitor(visitor);

            // Act
            tour.RemoveVisitor(visitor);

            // Assert
            Assert.IsFalse(tour.ExpectedVisitors.Contains(visitor));
        }

        [TestMethod]
        public void FindTourByVisitorTicketCode_FindsCorrectTour()
        {
            // Setup
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 40, 13, false, false, new Guide(Guid.NewGuid(), "John", "111"));
            tour.AddVisitor(visitor);
            Tour.TodaysTours.Add(tour);

            // Act
            var foundTour = Tour.FindTourByVisitorTicketCode("ABC123");

            // Assert
            Assert.AreEqual(tour, foundTour);
        }

        [TestMethod]
        public void SaveTours_SavesToursToFile()
        {
            // Arrange
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 40, 13, false, false, new Guide(Guid.NewGuid(), "John", "111"));
            Tour.TodaysTours.Add(tour);

            // Act
            Tour.SaveTours();

            // Assert
            Assert.IsTrue(File.Exists(Tour.JsonFilePath));
            var savedTours = JsonConvert.DeserializeObject<List<Tour>>(File.ReadAllText(Tour.JsonFilePath));
            Assert.AreEqual(Tour.TodaysTours.Count, savedTours.Count);

            // Cleanup
            if (File.Exists(Tour.JsonFilePath))
            {
                File.Delete(Tour.JsonFilePath);
            }
        }

        [TestMethod]
        public void LoadTours_LoadsToursFromFile()
        {
            // Arrange
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 40, 13, false, false, new Guide(Guid.NewGuid(), "John", "111"));
            Tour.TodaysTours.Add(tour);
            Tour.SaveTours();

            // Act
            Tour.LoadTours();

            // Assert
            Assert.AreEqual(1, Tour.TodaysTours.Count);
            Assert.AreEqual(tour.TourId, Tour.TodaysTours[0].TourId);

            // Cleanup
            if (File.Exists(Tour.JsonFilePath))
            {
                File.Delete(Tour.JsonFilePath);
            }
        }

    }


}

