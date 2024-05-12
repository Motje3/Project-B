using Newtonsoft.Json;

namespace MuseumTesting
{
    [TestClass]
    public class GuideTests
    {
        private const string GuideAssignmentsFilePath = "./JSON-Files/GuideAssignments.json";

        [TestMethod]
        public void Constructor_WithTourId_AddsTourToAssignedTourIds()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            // Act
            var guide = new Guide("John", tourId);

            // Assert
            Assert.IsTrue(guide.AssignedTourIds.Contains(tourId));
        }

        [TestMethod]
        public void AssignTour_WithNewTour_AddsTourToAssignedTourIds()
        {
            // Arrange
            var guide = new Guide("John");
            Guid tourId = Guid.NewGuid();

            // Act
            guide.AssignTour(tourId);

            // Assert
            Assert.IsTrue(guide.AssignedTourIds.Contains(tourId));
        }

        [TestMethod]
        public void LoadGuides_WithValidGuideAssignments_LoadsGuides()
        {
            // Arrange
            var guideAssignments = new List<dynamic>
            {
                new { GuideName = "John" },
                new { GuideName = "Alice" }
            };
            SaveGuideAssignmentsToFile(guideAssignments);

            // Act
            Guide.LoadGuides();

            // Assert
            Assert.AreEqual(2, Guide.AllGuides.Count);
            Assert.IsTrue(Guide.AllGuides.Any(g => g.Name == "John"));
            Assert.IsTrue(Guide.AllGuides.Any(g => g.Name == "Alice"));

            // Clean up
            DeleteGuideAssignmentsFile();
        }

        [TestMethod]
        public void ReassignGuideToTour_WithValidInput_AssignsGuideToTour()
        {
            // Arrange
            var guide = new Guide("John");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("Alice"));
            Tour.TodaysTours.Add(tour);
            Guide.AllGuides.Add(guide);

            // Simulate user input
            Console.SetIn(new StringReader("1\n1\n"));

            // Act
            Guide.ReassignGuideToTour();

            // Assert
            Assert.AreEqual(guide, tour.AssignedGuide);

            // Clean up
            Tour.TodaysTours.Clear();
            Guide.AllGuides.Clear();
        }

        private void SaveGuideAssignmentsToFile(List<dynamic> guideAssignments)
        {
            string jsonData = JsonConvert.SerializeObject(guideAssignments);
            File.WriteAllText(GuideAssignmentsFilePath, jsonData);
        }

        private void DeleteGuideAssignmentsFile()
        {
            if (File.Exists(GuideAssignmentsFilePath))
            {
                File.Delete(GuideAssignmentsFilePath);
            }
        }
    }
}
