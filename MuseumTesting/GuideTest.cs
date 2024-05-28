using System.Globalization;
using Newtonsoft.Json;
using ReservationSystem;

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
            var guidid = Guid.NewGuid();
            Guid tourId = Guid.NewGuid();

            // Act
            var guide = new Guide(guidid, "John", "111");

            // Assert
            Assert.IsTrue(guide.AssignedTourIds.Contains(guidid));
        }

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

        [TestMethod]
        public void AddVisitorLastMinuteTest()
        {
            // Arrange
            TourDataManager.SaveTours();
            Guid tourId1 = Guid.NewGuid();
            Guid tourId2 = Guid.NewGuid();

            Guide John = new Guide(tourId1, "John Doe", "111");
            Guide Alice = new Guide(tourId2, "Alice Jackson", "444");

            Visitor Bob = new Visitor("222");
            Visitor Mark = new Visitor("333");
            Visitor Carl = new Visitor("333");

            // Clear existing data
            TourTools.TodaysTours.Clear();
            Guide.AllGuides.Clear();

            // Add guides to the list
            Guide.AllGuides.Add(John);
            Guide.AllGuides.Add(Alice);

            // Create tours
            DateTime date = DateTime.Now;  // used to simulate today's tours
            
            // Note that test might fail between 22:00 (10:00 PM) - 00:00 (12:00 AM),
            // avoid this test between these time periods.
            var tour1 = new Tour(Guid.NewGuid(), new DateTime(date.Year, date.Month, date.Day, 22, 0, 0), 40, 13, false, false, John);
            var tour2 = new Tour(Guid.NewGuid(), new DateTime(date.Year, date.Month, date.Day, 23, 0, 0), 40, 13, false, false, Alice);

            TourTools.TodaysTours.Add(tour1);
            TourTools.TodaysTours.Add(tour2);
            TourDataManager.SaveTours();

            // Act
            John.AddVisitorLastMinute(Bob);
            John.AddVisitorLastMinute(Mark);
            Alice.AddVisitorLastMinute(Carl);

            // Add tours to today's tours

            // Assert
            // John should have 2 presentvistors (tour1)
            // Alice should have 1 presentvistors (tour2)
            Assert.AreEqual(2, TourTools.TodaysTours[0].ExpectedVisitors.Count);
            Assert.AreEqual(1, TourTools.TodaysTours[1].ExpectedVisitors.Count);
        }

        [TestMethod]
        public void StartUpcomingTour_NoUpcomingTours_ReturnsFalse()
        {
            // Arrange
            var guide = new Guide(Guid.NewGuid(), "John", "111");
            TourTools.TodaysTours.Clear();

            // Act
            var result = guide.StartUpcomingTour();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void StartUpcomingTour_WithUpcomingTour_ReturnsTrueAndAddsPresentVisitors()
        {
            // Arrange
            var guide = new Guide(Guid.NewGuid(), "John", "password123");
            Guide.AllGuides.Clear();
            Guide.AllGuides.Add(guide);
            
            var visitor = new Visitor("444");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now.AddMinutes(30), 40, 10, false, false, guide);
            tour.ExpectedVisitors.Add(visitor);
            TourTools.TodaysTours.Clear();
            TourTools.TodaysTours.Add(tour);

            using (var stringWriter = new StringWriter())
            {
                using (var stringReader = new StringReader(visitor.TicketCode + Environment.NewLine + "start" + Environment.NewLine))
                {
                    Console.SetOut(stringWriter);
                    Console.SetIn(stringReader);

                    // Act
                    var result = guide.StartUpcomingTour();

                    // Assert
                    Assert.IsTrue(result);
                    Assert.IsTrue(tour.PresentVisitors.Any(v => v.TicketCode == visitor.TicketCode));
                }
            }
        }
    }
}


