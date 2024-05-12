namespace MuseumTesting
{
    [TestClass]
    public class AdminBackEndTests
    {
        [TestMethod]
        public void ChangeTourCapacity_NotImplemented_ExceptionThrown()
        {
            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => AdminBackEnd.ChangeTourCapacity());
        }

        [TestMethod]
        public void ChangeTourTime_NotImplemented_ExceptionThrown()
        {
            // Act & Assert
            Assert.ThrowsException<NotImplementedException>(() => AdminBackEnd.ChangeTourTime());
        }

        [TestMethod]
        public void ReadPassword_ReturnsCorrectPassword()
        {
            // Arrange
            string expectedPassword = "test123";

            // Simulate user input
            Console.SetIn(new System.IO.StringReader(expectedPassword + Environment.NewLine));

            // Act
            string actualPassword = AdminBackEnd.ReadPassword();

            // Assert
            Assert.AreEqual(expectedPassword, actualPassword);
        }
    }
}
