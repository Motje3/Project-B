using Newtonsoft.Json;
using ReservationSystem;

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

        // [TestMethod]
        // public void ShowGuideMenuTest()
        // {
        //     // Act
            
        //     string tourChoice = "1";
        //     string visitorTicketCode = "1234567890";
        //     FakeWorld fakeWorld = new()
        //     {
        //         Today = new DateTime(2024, 6, 1),
        //         Files =
        //         {
        //             {"./JSON-Files/OnlineTickets.json", "[\"1234567890\"]"},
        //             {"./JSON-Files/TourSettings.json","{\"StartTime\": \"11:00:00\",\"EndTime\": \"16:15:00\",\"Duration\": 15,\"MaxCapacity\": 13}"},
        //             {"./JSON-Files/GuideAssignments.json","[{\"GuideName\": \"John Doe\",\"Tours\": [{\"StartTime\": \"11:00 AM\"},{\"StartTime\": \"12:15 PM\"},{\"StartTime\": \"01:30 PM\"},{\"StartTime\": \"02:45 PM\"},{\"StartTime\": \"04:00 PM\"}]},{\"GuideName\": \"Alice Johnson\",\"Tours\": [{\"StartTime\": \"11:15 AM\"},{\"StartTime\": \"12:30 PM\"},{\"StartTime\": \"01:45 PM\"},{\"StartTime\": \"03:00 PM\"},{\"StartTime\": \"04:15 PM\"}]},{\"GuideName\": \"Steve Brown\",\"Tours\": [{\"StartTime\": \"11:30 AM\"},{\"StartTime\": \"12:45 PM\"},{\"StartTime\": \"02:00 PM\"},{\"StartTime\": \"03:15 PM\"}]},{\"GuideName\": \"Mary Lee\",\"Tours\": [{\"StartTime\": \"11:45 AM\"},{\"StartTime\": \"01:00 PM\"},{\"StartTime\": \"02:15 PM\"},{\"StartTime\": \"03:30 PM\"}]},{\"GuideName\": \"Tom Clark\",\"Tours\": [{\"StartTime\": \"12:00 PM\"},{\"StartTime\": \"01:15 PM\"},{\"StartTime\": \"02:30 PM\"},{\"StartTime\": \"03:45 PM\"}]}]"}
        //         },
        //         LinesToRead = new()
        //         {
        //             "3"
        //         }
        //     };
        //     Program.World = fakeWorld;
        //     GuideMenuRL.Show();

        //     // Assert
        //     Assert.IsTrue(fakeWorld.LinesWritten.Contains("1 | View personal tours"));
        //     Assert.IsTrue(fakeWorld.LinesWritten.Contains("2 | Add visitor for next tour"));
        //     Assert.IsTrue(fakeWorld.LinesWritten.Contains("3 | Exit"));
        // }
    }
}

