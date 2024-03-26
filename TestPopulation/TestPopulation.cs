using Microsoft.VisualStudio.TestPlatform.Common.Utilities;
using Newtonsoft.Json;

namespace TestPopulation;

[TestClass]
public class UnitTest1
{
    [TestMethod]
    public void FileCreation()
    {
        bool FileExist = false;
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.JSONWriter(0, filePath, currentDate);  
        Assert.IsTrue(File.Exists(filePath));  // Check if (new) file is made
    }
    [TestMethod]
    public void TestTrackVistorOnebyOne()
    {
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.AddVistors(1); // add one visitor to JSON (+1)
        pop.AddVistors(1); // add one visitor to JSON (+1)
        pop.AddVistors(1); // add one visitor to JSON (+1)
        // make read JSON method to get an int from JSON
        int Result = pop.TotalVisitor % 3;  // logic here is that it gets and writes to JSON note that this test will fail if you us
        Assert.AreEqual(Result, 0);
    }
    [TestMethod]
    public void TestTrackVistorAllinOne()
    {
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.AddVistors(3); // add three visitors to JSON (+3)
        int Result = pop.TotalVisitor % 3;  
        Assert.AreEqual(Result, 0);
    }
}