using Newtonsoft.Json;
// namespace Population;  // uncomment this namespace to perform TestPopulation dont foget uncomment aftherwards 

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
    public void CreateTourHistory(string TourData) // default = 1
    {
        DateTime currentDate = DateTime.Today;
        // Specify the path for the new and existing JSON files
        // deffault location = bin\debug\net7.0
        string filePathRead = $"./JSON-Files/guidedTours.json"; // file to receive the guided tour data // assuming it resets evry day
        string filePathSave = $"./JSON-Files/TourData_{currentDate:dd-MM-yyyy}.json";
        try
        {
            StreamReader reader = new StreamReader(filePathRead);
            string people = reader.ReadToEnd();  // JSON will always return a string.  
            reader.Close();
            JSONWriter(TourData, filePathSave, currentDate);
        }
        catch(FileNotFoundException ex)
        { 
            Console.WriteLine($"File not found, tour history can't be saved: {ex}");
        }
        catch(FormatException ex)
        {
            Console.WriteLine($"File curroption cant format string: {ex}");
        }
        catch(DirectoryNotFoundException)
        {
            Console.WriteLine("WARNING! no directory to store total visitor DATA,");
            Console.WriteLine("DATA will not be saved");
        }
        Console.WriteLine();
    }
    public void JSONWriter(string TourData, string filePath, DateTime currentDate)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string jsonData = TourData;
        writer.Write(JsonConvert.SerializeObject(jsonData, Formatting.Indented));
        writer.Close();
    }
}