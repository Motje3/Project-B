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


    }
}

