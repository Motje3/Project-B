namespace MuseumTesting
{
    [TestClass]
    public class VisitorTests
    {
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
            var visitor = new Visitor("ABC123");
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"));
            tour.AddVisitor(visitor);

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
            string ticketCode = "DEF456";
            var visitor = new Visitor(ticketCode);
            var tour = new Tour(Guid.NewGuid(), DateTime.Now, 60, 30, false, false, new Guide("John"));
            tour.AddVisitor(visitor);

            // Act
            var foundVisitor = Visitor.FindVisitorByTicketCode(ticketCode);

            // Assert
            Assert.IsNotNull(foundVisitor);
            Assert.AreEqual(ticketCode, foundVisitor.TicketCode);
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
            string ticketCode = "JKL012";
            var visitor = new Visitor(ticketCode);
            var tour = new Tour(Guid.NewGuid(), DateTime.Now.AddHours(1), 60, 30, false, false, new Guide("John"));
            tour.AddVisitor(visitor);

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
