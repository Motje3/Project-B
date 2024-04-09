using Newtonsoft.Json;
public class ToursHistory
{
    public void LogReservation(string name, string ticketcode, DateTime tourtime, bool IsCancel)  // true for cancel tour, false for regester tour.
    {
        // bool RegOrCan: true for Registration, false for Canelation
        DateTime currentDate = DateTime.Today;
        DateTime logtime = DateTime.Now;
        string strlogtime = logtime.ToString("HH:mm");  // This ensures only the clock will be displayed
        string header = $"Reservation logger, Date: {currentDate:dd-MM-yyyy}";
        string directoryPath = "./Logs/TourReservationLog"; 
        CheckDirectory(directoryPath);  // create directory if it does no exist
        string textPath = $"./Logs/TourReservationLog/{currentDate:dd-MM-yyyy}_TourReservationLog.txt";  // create new textfile at current day
        string Log = IsCancel
            // checks bool to log aproperiate string
            ? $"{strlogtime}: {ticketcode} {name} canceled his/her tour on {tourtime}"  // true
            : $"{strlogtime}: {ticketcode} {name} registered for tour on {tourtime}";  // false

        WriteLog(textPath, Log, header);
    }
    public void LogTourchange(string name, string ticketcode, DateTime OldTourTime, DateTime NewTourTime) // old for cancelation, new for Regestration
    {
        // logic here is that it will cancel, and register with same ticket code.
        this.LogReservation(name, ticketcode, OldTourTime, false);
        this.LogReservation(name, ticketcode, NewTourTime, true);
    }
    public void LogGuidedTour(string name, string ticketcode, DateTime tourtime)  // this logs joined tours in DIFFRENT folder 
    {
        // bool RegOrCan: true for Registration, false for Canelation
        DateTime currentDate = DateTime.Today;
        DateTime logtime = DateTime.Now;
        string strlogtime = logtime.ToString("HH:mm");  // This ensures only the clock will be displayed
        string strtourtime = tourtime.ToString("HH:mm");
        string header = $"GuidedTour logger, Date: {currentDate:dd-MM-yyyy}";
        string directoryPath = "./Logs/GuidedTourLog";  
        CheckDirectory(directoryPath);  // create directory if it does not exist
        string textPath = $"./Logs/GuidedTourLog/{currentDate:dd-MM-yyyy}_GuidedTourLog.txt";  // create new textfile at current day
        string Log = $"{strlogtime}: TourTime[{strtourtime}] {ticketcode} {name} has joined the tour";  // this log will be written/appended to log

        WriteLog(textPath, Log, header);
    }

    

    private void WriteLog(string filePath, string log, string header)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                if (writer.BaseStream.Length == 0) // checks if header already pressent
                {
                    // Write header if the file is empty
                    writer.WriteLine(header);
                }

                // Write log message
                writer.WriteLine(log);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
        }
    }
    private void CheckDirectory(string directoryPath)
    {
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }


    public void GetPop()  // retrieve data by selecting start and end dates to search total amount of visitors
    {
        
        DateTime startDate;
        DateTime endDate;

        // Read start date until a valid date is entered
        while (true)
        {
            Console.WriteLine("Enter start date (yyyy-MM-dd):");  // do to limitatation of console engine, it must be in american format
            string startDateString = Console.ReadLine();
            Console.WriteLine();

            if (DateTime.TryParse(startDateString, out startDate))
            {
                break; // Break the loop if parsing is successful
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
                // continue untill Parseble
            }
        }

        // Read end date until a valid date is entered
        while (true)
        {
            Console.WriteLine("Enter end date (yyyy-MM-dd):");  // do to limitatation of console engine, it must be in american format
            string endDateString = Console.ReadLine();
            Console.WriteLine();

            if (DateTime.TryParse(endDateString, out endDate))
            {
                break; // Break the loop if parsing is successful
            }
            else
            {
                Console.WriteLine("Invalid date format. Please enter a date in yyyy-MM-dd format.");
                // continue untill Parseble
            }
        }
        
        List<string> datestosearch = new List<string>();

        while(startDate < endDate)
        {
            string datetoadd = startDate.ToString("dd-MM-yyyy");
            datestosearch.Add(datetoadd); // add date to list
            startDate = startDate.AddDays(1); // add a day to DateTime object
        }
        int totalpop = 0;

        foreach(string date in datestosearch)
        {
            
            string textPath = $"./Logs/GuidedTourLog/{date}_GuidedTourLog.txt";  // loops trought evry log that has the name: dd-MM-yyyy_GuidedTourLog.txt"

            if (File.Exists(textPath)) // checks if file exist when looping trought the list of dates, ignore futher action if does not exist.
            {
                using (StreamReader reader = new StreamReader(textPath))
                try
                {
                    int lineCount = -1; // logic is that the header is a line so start with -1

                    // Read the file line by line until the end
                    while (reader.ReadLine() != null)
                    {
                        lineCount++; // Increment line count for each line read
                    }

                    if (!(lineCount == -1)) // failsave to prevent displaying currupted data (empty file)
                    {
                        Console.WriteLine($"{date}: {lineCount} visitors has joined a tour");
                        totalpop += lineCount;
                    }
                    lineCount = -1;  // reset count for next file            
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while writing to the log file: {ex.Message}");
                }
            }
        }   
        string strEndDate = endDate.ToString("dd-MM-yyyy");  // for end date display
        Console.WriteLine($"\nTotal amount of visitors from {datestosearch[0]} to {strEndDate}: {totalpop}");
        // tested:

        // Enter start date (yyyy-MM-dd):
        // 2024-01-01

        // Enter end date (yyyy-MM-dd):
        // 2025-01-01

        // 05-04-2024: 3 visitors has joined a tour
        // 06-04-2024: 9 visitors has joined a tour
        // 09-04-2024: 4 visitors has joined a tour
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
    private void JSONWriter(string TourData, string filePath, DateTime currentDate)
    {
        StreamWriter writer = new StreamWriter(filePath);
        string jsonData = TourData;
        Console.WriteLine(jsonData);
        writer.Write(JsonConvert.SerializeObject(jsonData));
        writer.Close();
    }
}