using Newtonsoft.Json;

namespace MuseumTesting
{
    [TestClass]
    public class GuideMenuBackEndTests
    {
        private const string CredentialsFilePath = "./JSON-Files/GidsCredentials.json";

        [TestMethod]
        public void AuthenticateGuide_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var credentials = new List<GuideMenuBackEnd.Credential>
            {
                new GuideMenuBackEnd.Credential { Password = "password1" },
                new GuideMenuBackEnd.Credential { Password = "password2" }
            };
            SaveCredentialsToFile(credentials);
            string validPassword = "password1";

            // Act
            bool isAuthenticated = GuideMenuBackEnd.AuthenticateGuide(validPassword);

            // Assert
            Assert.IsTrue(isAuthenticated);

            // Clean up
            DeleteCredentialsFile();
        }

        [TestMethod]
        public void AuthenticateGuide_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var credentials = new List<GuideMenuBackEnd.Credential>
            {
                new GuideMenuBackEnd.Credential { Password = "password1" },
                new GuideMenuBackEnd.Credential { Password = "password2" }
            };
            SaveCredentialsToFile(credentials);
            string invalidPassword = "incorrect";

            // Act
            bool isAuthenticated = GuideMenuBackEnd.AuthenticateGuide(invalidPassword);

            // Assert
            Assert.IsFalse(isAuthenticated);

            // Clean up
            DeleteCredentialsFile();
        }

        private void SaveCredentialsToFile(List<GuideMenuBackEnd.Credential> credentials)
        {
            string jsonData = JsonConvert.SerializeObject(credentials);
            File.WriteAllText(CredentialsFilePath, jsonData);
        }

        private void DeleteCredentialsFile()
        {
            if (File.Exists(CredentialsFilePath))
            {
                File.Delete(CredentialsFilePath);
            }
        }
    }
}
