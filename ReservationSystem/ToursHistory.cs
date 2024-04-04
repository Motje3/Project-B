using Newtonsoft.Json;
// namespace SavedToursHistory;  // uncomment this namespace to perform TestPopulation dont foget uncomment aftherwards 

public class SavedToursHistory
{
    public DateTime? Day;  // tracks current day
    
    public SavedToursHistory()
    {
        Day = null;
    }
    
    public void LogCancelation(string name, string ticketcode, DateTime TourTime) // TourTime can be string depending on impletation
    {
        // need cancelation data
        DateTime currentDate = DateTime.Today;
        DateTime logtime = DateTime.Now;
        string textPath = $"./Logs/TourData_{currentDate:dd-MM-yyyy}.txt";
        string Log = $"{logtime}: {ticketcode} {name} canceled his/her tour on {TourTime}";
        WriteLog(textPath, Log);
    }
    public void LogRegestration(string name, string ticketcode, DateTime TourTime) // TourTime can be string depending on impletation
    { 
        // need registration data
        DateTime currentDate = DateTime.Today;
        DateTime logtime = DateTime.Now;
        string textPath = $"./Logs/TourData_{currentDate:dd-MM-yyyy}.txt";
        string Log = $"{logtime}: {ticketcode} {name} registerd for tour on {TourTime}";
        WriteLog(textPath, Log);
    }
    public void LogTourchange(string name, string ticketcode, DateTime OldTourTime, DateTime NewTourTime) // old for cancelation, new for Regestration
    {
        // logic here is that it will cancel, and register with same ticket code.
        this.LogCancelation(name, ticketcode, OldTourTime);
        this.LogRegestration(name, ticketcode, NewTourTime);
    }
    private void WriteLog(string filePath, string log)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(log);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
        }
    }
    public void CreateTourHistory()
    {
        DateTime currentDate = DateTime.Today;
        // Specify the path for the new and existing JSON files
        
        string filePathRead = $"./JSON-Files/guidedTours.json"; // read guided tour data and than store it in other folder // assuming it resets evry day
        string filePathSave = $"./JSON-Files/TourData_{currentDate:dd-MM-yyyy}.json"; // copy guidedTours datato this file each day // deffault location = bin\debug\net7.0
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