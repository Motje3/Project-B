using Newtonsoft.Json;

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
                Duration = 60, // Duration in minutes
                MaxCapacity = 30
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
        public void InitializeTours_WithSettingsAndAssignments_CreatesTours()
        {
            // Act
            Tour.InitializeTours();

            // Assert
            Assert.IsTrue(Tour.TodaysTours.Count > 0);
            Assert.AreEqual(Tour.TodaysTours[0].MaxCapacity, 13);
        }

        [TestMethod]
        public void AddVisitor_WithValidConditions_AddsVisitor()
        {
            // Setup
            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"));

            // Act
            tour.AddVisitor(visitor);

            // Assert
            Assert.IsTrue(tour.ExpectedVisitors.Contains(visitor));
        }

        [TestMethod]
        public void RemoveVisitor_RemovesExpectedVisitor()
        {
            // Setup
            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"));
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
            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"));
            tour.AddVisitor(visitor);
            Tour.TodaysTours.Add(tour);

            // Act
            var foundTour = Tour.FindTourByVisitorTicketCode("ABC123");

            // Assert
            Assert.AreEqual(tour, foundTour);
        }
    }
}

