using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class SysTestChangeReservation
    {
        [TestCleanup]
        public void CleanUp()
        {
            try
            {
                File.Delete(Tour.JsonFilePath);
            }
            catch (DirectoryNotFoundException)
            {

            }
        }

        [TestMethod]
        public void TestVisitorChangeReservationMenuSucceful()
        { 

        }
    }
}