using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class GuideMenuBackEndTests
    {
        private const string CredentialsFilePath = "./JSON-Files/GidsCredentials.json";
        [TestInitialize]
        public void setUp()
        {
            Guide.AllGuides.Clear();
        }

        [TestMethod]
        public void AuthenticateGuide_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            string validPassword = "password1";
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/GuideAssignments.json", "[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+validPassword+"\",\"Tours\":[]}]"}
                }
            };
            Program.World = world;
            Guide.LoadGuides();

            // Act
            bool isAuthenticated = Guide.AuthenticateGuide(validPassword) != null;

            // Assert
            Assert.IsTrue(isAuthenticated);

        }

        [TestMethod]
        public void AuthenticateGuide_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            string invalidPassword = "1234";
            string validPassword = "4321";
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/GuideAssignments.json", "[{\"GuideId\":\"6c8cb11c-cf42-4606-8778-2831c5c5fda8\",\"GuideName\":\"John Doe\",\"Password\":\""+validPassword+"\",\"Tours\":[]}]"}
                }
            };
            Guide.LoadGuides();

            // Act
            bool isNotAuthenticated = Guide.AuthenticateGuide(invalidPassword) == null;

            // Assert
            Assert.IsTrue(isNotAuthenticated);

        }
    }
}

