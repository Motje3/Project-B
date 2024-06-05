using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class TicketTests
    {

        [TestMethod]
        public void LoadTickets_FileExists_TicketsLoaded()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"1111\",\"2222\",\"3333\"]"}
                }
            };
            Program.World = world;

            // Act - reload tickets to simulate reading from file
            Ticket.LoadTickets();

            // Assert
            Assert.AreEqual(3, Ticket.Tickets.Count);
            Assert.AreEqual("1111", Ticket.Tickets[0]);
        }

        [TestMethod]
        public void IsCodeValid_ValidCode_ReturnsTrue()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"1111\",\"2222\",\"3333\"]"}
                }
            };
            Program.World = world;
            Ticket.LoadTickets();

            // Act
            bool isValid = Ticket.IsCodeValid("1111");

            // Assert
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void IsCodeValid_InvalidCode_ReturnsFalse()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/OnlineTickets.json", $"[\"1111\",\"2222\",\"3333\"]"}
                }
            };
            Program.World = world;
            Ticket.LoadTickets();

            // Act
            var isValid = Ticket.IsCodeValid("INVALID001");

            // Assert
            Assert.IsFalse(isValid);
        }
    }
}

