using Moq;

namespace MuseumTesting
{
    [TestClass]
    public class MenuLogicTests
    {
        [TestMethod]
        public void HandleFullMenuChoice_InvalidChoice_ReturnsTrue()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var menuLogic = new MenuLogic();

            // Act
            bool result = menuLogic.HandleFullMenuChoice("invalid", visitor);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void JoinTour_ValidTourChoice_SuccessfullyJoinsTour()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var menuLogic = new MenuLogic();
            var mockTour = new Mock<Tour>();
            mockTour.Setup(tour => tour.Completed).Returns(false);
            mockTour.Setup(tour => tour.Deleted).Returns(false);
            mockTour.Setup(tour => tour.ExpectedVisitors.Count).Returns(0);
            mockTour.Setup(tour => tour.MaxCapacity).Returns(30);
            Tour.TodaysTours.Add(mockTour.Object);

            // Simulate user input
            Console.SetIn(new StringReader("1\n"));

            // Act
            bool result = MenuLogic.JoinTour(visitor);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, mockTour.Object.ExpectedVisitors.Count);

            // Clean up
            Tour.TodaysTours.Clear();
        }

        [TestMethod]
        public void JoinTour_InvalidTourChoice_FailsToJoinTour()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var input = new StringReader("invalid\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            Tour.TodaysTours.Add(new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"))); // Adding a sample tour

            // Act
            bool result = MenuLogic.JoinTour(visitor);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(output.ToString().Contains("Invalid choice, please choose a valid tour number"));

            // Cleanup
            Tour.TodaysTours.Clear(); // Resetting global state
            Console.SetIn(Console.In); // Reset Console In
            Console.SetOut(Console.Out); // Reset Console Out
        }


        [TestMethod]
        public void ChangeTour_ValidTourChoice_SuccessfullyChangesTour()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var menuLogic = new MenuLogic();
            var mockCurrentTour = new Mock<Tour>();
            var mockNewTour = new Mock<Tour>();
            mockCurrentTour.Setup(tour => tour.StartTime).Returns(DateTime.Now);
            mockCurrentTour.Setup(tour => tour.ExpectedVisitors.Contains(visitor)).Returns(true);
            mockNewTour.Setup(tour => tour.StartTime).Returns(DateTime.Now.AddHours(1));
            mockNewTour.Setup(tour => tour.ExpectedVisitors.Count).Returns(0);
            mockNewTour.Setup(tour => tour.MaxCapacity).Returns(30);
            Tour.TodaysTours.Add(mockCurrentTour.Object);
            Tour.TodaysTours.Add(mockNewTour.Object);

            // Simulate user input
            Console.SetIn(new StringReader("1\n"));

            // Act
            MenuLogic.ChangeTour(visitor);

            // Assert
            Assert.AreEqual(0, mockCurrentTour.Object.ExpectedVisitors.Count);
            Assert.AreEqual(1, mockNewTour.Object.ExpectedVisitors.Count);

            // Clean up
            Tour.TodaysTours.Clear();
        }

        [TestMethod]
        public void CancelTour_WithExistingReservation_SuccessfullyCancelsTour()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var menuLogic = new MenuLogic();
            var mockTour = new Mock<Tour>();
            mockTour.Setup(tour => Tour.FindTourByVisitorTicketCode(visitor.TicketCode)).Returns(mockTour.Object);
            Tour.TodaysTours.Add(mockTour.Object);

            // Act
            bool result = MenuLogic.CancelTour(visitor);

            // Assert
            Assert.IsFalse(result);
            Assert.AreEqual(0, mockTour.Object.ExpectedVisitors.Count);

            // Clean up
            Tour.TodaysTours.Clear();
        }

        [TestMethod]
        public void CancelTour_WithoutExistingReservation_FailsToCancelTour()
        {
            // Arrange
            var visitor = new Visitor("ABC123");
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            bool result = MenuLogic.CancelTour(visitor);

            // Assert
            Assert.IsTrue(result);
            Assert.IsTrue(output.ToString().Contains("Error: Unable to find the tour"));

            // Cleanup
            Console.SetOut(Console.Out); // Reset Console Out
        }
    }
}

