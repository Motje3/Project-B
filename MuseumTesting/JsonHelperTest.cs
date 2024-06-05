using Newtonsoft.Json;
using ReservationSystem;

namespace MuseumTesting
{
    [TestClass]
    public class JsonHelperTests
    {
        private string filePath = "./JSON-Files/TestData.json";

        [TestInitialize]
        public void SetUp()
        {
            // Ensure the directory exists
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));

            // Create a temporary data file with initial JSON content
            var testData = new List<string> { "ExampleData1", "ExampleData2" };
            string json = JsonConvert.SerializeObject(testData);
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
        public void LoadFromJson_ValidFile_ReturnsCorrectData()
        {
            Program.World = new RealWorld();
            // Act
            var data = JsonHelper.LoadFromJson<List<string>>(filePath);

            // Assert
            Assert.AreEqual(2, data.Count);
            Assert.AreEqual("ExampleData1", data[0]);
        }

        [TestMethod]
        public void SaveToJson_ValidData_FileUpdated()
        {
            // Arrange
            var newData = new List<string> { "NewExampleData1", "NewExampleData2" };

            // Act
            JsonHelper.SaveToJson(newData, filePath);

            // Reload data from file
            var savedData = JsonHelper.LoadFromJson<List<string>>(filePath);

            // Assert
            Assert.AreEqual(2, savedData.Count);
            Assert.AreEqual("NewExampleData1", savedData[0]);
        }
    }
}

