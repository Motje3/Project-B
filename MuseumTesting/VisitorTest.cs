namespace MuseumTesting
{
    [TestClass]
    public class VisitorTests
    {

        [TestInitialize]
        public void TestInitialize()
        {
            Tour.TodaysTours.Clear();  // Ensures a clean state before each test
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Tour.TodaysTours.Clear();  // Cleans up after each test
        }

        [TestMethod]
        public void Constructor_CreatesVisitorWithTicketCode()
        {
            // Arrange
            string ticketCode = "ABC123";

            // Act
            var visitor = new Visitor(ticketCode);

            // Assert
            Assert.AreEqual(ticketCode, visitor.TicketCode);
        }

        [TestMethod]
        public void HasReservation_WithReservation_ReturnsTrue()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"));
            tour.ExpectedVisitors.Add(visitor);
            Tour.TodaysTours.Add(tour);

            // Act
            bool hasReservation = visitor.HasReservation(visitor);

            // Assert
            Assert.IsTrue(hasReservation);
        }

        [TestMethod]
        public void HasReservation_WithoutReservation_ReturnsFalse()
        {
            // Arrange
            var visitor = new Visitor("XYZ789");

            // Act
            bool hasReservation = visitor.HasReservation(visitor);

            // Assert
            Assert.IsFalse(hasReservation);
        }

        [TestMethod]
        public void FindVisitorByTicketCode_WithExistingVisitor_ReturnsVisitor()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("DEF456");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide(tourId, "John", "111"));
            tour.ExpectedVisitors.Add(visitor);
            Tour.TodaysTours.Add(tour);

            // Act
            var foundVisitor = Visitor.FindVisitorByTicketCode("DEF456");

            // Assert
            Assert.IsNotNull(foundVisitor);
            Assert.AreEqual("DEF456", foundVisitor.TicketCode);
        }


        [TestMethod]
        public void FindVisitorByTicketCode_WithNonExistingVisitor_ReturnsNull()
        {
            // Arrange
            string ticketCode = "GHI789";

            // Act
            var foundVisitor = Visitor.FindVisitorByTicketCode(ticketCode);

            // Assert
            Assert.IsNull(foundVisitor);
        }

        [TestMethod]
        public void GetCurrentReservation_WithExistingReservation_ReturnsReservationDetails()
        {
            // Arrange
            Guid tourId = Guid.NewGuid();

            var visitor = new Visitor("JKL012");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now.AddHours(1), 60, 30, false, false, new Guide(tourId, "John", "111"));
            tour.ExpectedVisitors.Add(visitor);
            Tour.TodaysTours.Add(tour);

            // Act
            string reservationDetails = Visitor.GetCurrentReservation(visitor);

            // Assert
            Assert.IsTrue(reservationDetails.Contains("Your current reservation is at"));
        }

        [TestMethod]
        public void GetCurrentReservation_WithoutReservation_ReturnsNoReservationMessage()
        {
            // Arrange
            string ticketCode = "MNO345";
            var visitor = new Visitor(ticketCode);

            // Act
            string reservationDetails = Visitor.GetCurrentReservation(visitor);

            // Assert
            Assert.IsTrue(reservationDetails.Contains("You currently have no reservation"));
        }
    }
}

