using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace MuseumTesting.Tests
{
    [TestClass]
    public class TicketTests
    {
        private string filePath = "./JSON-Files/OnlineTickets.json";

        [TestInitialize]
        public void SetUp()
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Create a temporary data file with initial JSON content
            var ticketCodes = new List<string> { "TICKET001", "TICKET002" };
            string json = JsonConvert.SerializeObject(ticketCodes);
            File.WriteAllText(filePath, json);

            // Reload the Ticket class to refresh the static Tickets list
            Ticket.LoadTickets();
        }

        [TestCleanup]
        public void CleanUp()
        {
            // Delete the test file to clean up
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }

        [TestMethod]
        public void LoadTickets_FileExists_TicketsLoaded()
        {
            // Act - reload tickets to simulate reading from file
            Ticket.LoadTickets();

            // Assert
            Assert.AreEqual(2, Ticket.Tickets.Count);
            Assert.AreEqual("TICKET001", Ticket.Tickets[0]);
        }

        [TestMethod]
        public void IsCodeValid_ValidCode_ReturnsTrue()
        {
            // Act
            var isValid = Ticket.IsCodeValid("TICKET001");

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsCodeValid_InvalidCode_ReturnsFalse()
        {
            // Act
            var isValid = Ticket.IsCodeValid("INVALID001");

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}
