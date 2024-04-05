using Newtonsoft.Json;
// namespace SavedToursHistory;  // uncomment this namespace to perform TestPopulation dont foget uncomment aftherwards 

public class SavedToursHistory
{
    public DateTime? Day;  // tracks current day
    
    public SavedToursHistory()
    {
        Day = null;
    }
    
    public void LogReservation(string name, string ticketcode, DateTime TourTime, bool IsCancel)  // true for cancel tour, false for regester tour.
    {
        // bool RegOrCan: true for Registration, false for Canelation
        DateTime currentDate = DateTime.Today;
        DateTime logtime = DateTime.Now;
        string directoryPath = "./Logs/TourReservationLog"; 
        CheckDirectory(directoryPath);  // create directory if it does no exist
        string textPath = $"./Logs/TourReservationLog/{currentDate:dd-MM-yyyy}_TourReservationManager.txt";
        string Log = IsCancel
            // checks bool to log aproperiate string
            ? $"{logtime}: {ticketcode} {name} canceled his/her tour on {TourTime}" 
            : $"{logtime}: {ticketcode} {name} registered for tour on {TourTime}";

        WriteLog(textPath, Log);
    }
    public void LogTourchange(string name, string ticketcode, DateTime OldTourTime, DateTime NewTourTime) // old for cancelation, new for Regestration
    {
        // logic here is that it will cancel, and register with same ticket code.
        this.LogReservation(name, ticketcode, OldTourTime, false);
        this.LogReservation(name, ticketcode, NewTourTime, true);
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
    public void CheckDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
    
    // this method MIGHT not be used.
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
        Console.WriteLine(jsonData);
        writer.Write(JsonConvert.SerializeObject(jsonData));
        writer.Close();
    }
}