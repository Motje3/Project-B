namespace Population;

[TestClass]
public class TestPopulation;

{
    [TestMethod]
    public void TestFileCreation()
    {
        bool FileExist = false;
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.JSONWriter(0, filePath, currentDate);  
        Assert.IsTrue(File.Exists(filePath));  // Check if file is made
    }
    public void TestTrackVistorOnebyOne()
    {
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.AddVistors(1) // add one visitor to JSON (+1)
        pop.AddVistors(1) // add one visitor to JSON (+1)
        pop.AddVistors(1) // add one visitor to JSON (+1)
        // make read JSON method to get an int from JSON
        Result = Totalvisitor % 3
        Assert.IsEqual(Result, 0)
    }
    public void TestTrackVistorAllinOne()
    {
        Population pop = new Population();
        DateTime currentDate = DateTime.Today;
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        pop.AddVistors(3) // add three visitors to JSON (+3)
        // make read JSON method to get an int from JSON
        Result = Totalvisitor % 3
        Assert.IsEqual(Result, 0)
    }
}