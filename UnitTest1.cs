using Microsoft.VisualStudio.TestPlatform.Common.Utilities;

namespace TestPopulation;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void TestMethod1()
    {
        bool FileExist = false;
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.JSONWriter(0, filePath, currentDate);  
        Assert.IsTrue(File.Exists(filePath));  // Check if file is made
    }
}