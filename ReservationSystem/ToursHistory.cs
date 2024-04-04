using Newtonsoft.Json;
// namespace SavedToursHistory;  // uncomment this namespace to perform TestPopulation dont foget uncomment aftherwards 

public class SavedToursHistory
{
    public DateTime? Day;  // tracks current day
    
    public SavedToursHistory()
    {
        Day = null;
    }
    public void LogCancelation()
    {
        string textpath;
    }
    public void LogRegestration()
    {
        string textpath;
    }
    public void CreateTourHistory() // default = 1
    {
        DateTime currentDate = DateTime.Today;
        // Specify the path for the new and existing JSON files
        
        string filePathRead = $"./JSON-Files/guidedTours.json"; // read guided tour data and than store it in other folder // assuming it resets evry day
        string filePathSave = $"./JSON-Files/TourData_{currentDate:dd-MM-yyyy}.json"; // copy guidedTours data to this file each day // deffault location = bin\debug\net7.0
        try
        {
            StreamReader reader = new StreamReader(filePathRead);
            string TourData = reader.ReadToEnd();  // JSON will always return a string.  
            reader.Close();
            JSONWriter(TourData, filePathSave, currentDate);
        }
        catch(FileNotFoundException ex)
        {
            Console.WriteLine($"guidedTours.json file not found");
            Console.WriteLine($"DATA will not be saved \n{ex}");
        }
        catch(FormatException ex)
        {
            Console.WriteLine("File curroption can't format string");
            Console.WriteLine($"DATA will not be saved \n{ex}");
        }
        catch(DirectoryNotFoundException ex)
        {
            Console.WriteLine("WARNING! no directory to store total visitor DATA,");
            Console.WriteLine($"DATA will not be saved \n{ex}");
        }
        Console.WriteLine();
    }
    public void JSONWriter(string TourData, string filePath, DateTime currentDate)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string jsonData = TourData;
        writer.Write(JsonConvert.SerializeObject(jsonData));
        writer.Close();
    }
}