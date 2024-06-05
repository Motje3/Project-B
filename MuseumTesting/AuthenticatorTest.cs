using ReservationSystem;
namespace MuseumTesting
{
    [TestClass]
    public class AuthenticatorTests
    {
        private string filePath = "./JSON-Files/AdminCredentials.json";

        [TestInitialize]
        public void SetUp()
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            // Create a temporary credential file
            var credentials = new List<Authenticator.Credential>
            {
                new Authenticator.Credential { Username = "guide1", Password = "pass1" }
            };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(credentials);
            File.WriteAllText(filePath, json);
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
        public void AuthenticateUser_ValidPassword_ReturnsTrue()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/AdminCredentials.json", "[{\"Password\":\"pass1\"},{\"Password\":\"pass2\"},{\"Password\":\"12\"}]"},
                }
            };
            Program.World = world;

            // Act
            var result = Authenticator.AuthenticateUser("pass1");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthenticateUser_InvalidPassword_ReturnsFalse()
        {
            // Arrange
            FakeWorld world = new()
            {
                Files =
                {
                    {"./JSON-Files/AdminCredentials.json", "[{\"Password\":\"pass1\"},{\"Password\":\"pass2\"},{\"Password\":\"12\"}]"},
                }
            };
            Program.World = world;

            // Act
            var result = Authenticator.AuthenticateUser("wrongpassword");

            // Assert
            Assert.IsFalse(result);
        }
    }
}

