using Moq;

namespace MuseumTesting
{
    [TestClass]
    public class MenuLogicTests
    {/*
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
            Guid tourId = Guid.NewGuid();


            var visitor = new Visitor("ABC123");
            Tour.TodaysTours.Add(new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"))); // Ensure there is at least one tour to join

            var input = new StringReader("1\n"); // Simulate choosing the first tour
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            bool result = MenuLogic.JoinTour(visitor);

            // Assert
            Assert.IsTrue(result);
            Assert.AreEqual(1, Tour.TodaysTours[0].ExpectedVisitors.Count); // Check if visitor was added

            // Cleanup
            Tour.TodaysTours.Clear();
            Console.SetIn(Console.In);
            Console.SetOut(Console.Out);
        }

        [TestMethod]
        public void JoinTour_InvalidTourChoice_FailsToJoinTour()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var input = new StringReader("invalid\n");
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            Tour.TodaysTours.Add(new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"))); // Adding a sample tour

            // Act
            bool result = MenuLogic.JoinTour(visitor);

            // Assert
            Assert.IsFalse(result);
            Assert.IsTrue(output.ToString().Contains("Invalid choice. please choose a valid tour number"));

            // Cleanup
            Tour.TodaysTours.Clear(); // Resetting global state
            Console.SetIn(Console.In); // Reset Console In
            Console.SetOut(Console.Out); // Reset Console Out
        }


        [TestMethod]
        public void ChangeTour_ValidTourChoice_SuccessfullyChangesTour()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var initialTour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"));
            var newTour = new Tour(Guid.NewGuid(), DateTime.Now.AddHours(2), 60, 30, false, false, new Guide(tourId, "John", "111"));
            initialTour.AddVisitor(visitor);
            Tour.TodaysTours.AddRange(new List<Tour> { initialTour, newTour });

            var input = new StringReader("2\n"); // Simulate choosing the second tour to change to
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            MenuLogic.ChangeTour(visitor);

            // Assert
            Assert.IsTrue(newTour.ExpectedVisitors.Contains(visitor));
            Assert.IsFalse(initialTour.ExpectedVisitors.Contains(visitor));

            // Cleanup
            Tour.TodaysTours.Clear();
            Console.SetIn(Console.In);
            Console.SetOut(Console.Out);
        }

        [TestMethod]
        public void CancelTour_WithExistingReservation_SuccessfullyCancelsTour()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"));
            tour.AddVisitor(visitor);
            Tour.TodaysTours.Add(tour);

            var input = new StringReader(""); // No input needed for cancellation
            Console.SetIn(input);
            var output = new StringWriter();
            Console.SetOut(output);

            // Act
            bool result = MenuLogic.CancelTour(visitor);

            // Assert
            Assert.IsFalse(result);
            Assert.IsFalse(tour.ExpectedVisitors.Contains(visitor)); // Ensure visitor was removed

            // Cleanup
            Tour.TodaysTours.Clear();
            Console.SetIn(Console.In);
            Console.SetOut(Console.Out);
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
        }*/
    }
}

