using Newtonsoft.Json;
// namespace Population;  // uncomment this namespace to perform TestPopulation dont foget uncomment aftherwards 

public class Population
{
    public int TotalVisitor;  // tracks the visitor count of the day
    public DateTime? Day;  // tracks current day to reset the amount of the day
    
    public Population()
    {
        TotalVisitor = 0;
        Day = null;
    }
    
    public void AddVistors(int amount = 1) // default = 1
    {
        DateTime currentDate = DateTime.Today;
        // Specify the path for the new and existing JSON files
        // deffault location = bin\debug\net7.0
        string filePath = $"Population_on_day_{currentDate:dd-MM-yyyy}.json";
        try
        {
            StreamReader reader = new StreamReader(filePath);
            string people = reader.ReadToEnd();  // JSON will always return a string.  
            int Intpeople = Convert.ToInt32(people);  // Convert string to int
            TotalVisitor = Intpeople;  // initilize Total visitor of current day on reboot
            reader.Close();
            JSONWriter(amount, filePath, currentDate);
        }
        catch(FileNotFoundException)
        { 
            TotalVisitor = 0;
            Console.WriteLine("New day creating new file");
            JSONWriter(amount, filePath, currentDate);
        }
        catch(FormatException)
        {
            TotalVisitor = 0;  
            Console.WriteLine("File curroption deffaulting to 0");
            JSONWriter(amount, filePath, currentDate);
        }
        catch(DirectoryNotFoundException)
        {
            Console.WriteLine("WARNING! no directory to store total visitor DATA,");
            Console.WriteLine("DATA will not be saved");
        }
        Console.WriteLine();
    }
    public void JSONWriter(int amount, string filePath, DateTime currentDate)
    {
        StreamWriter writer = new StreamWriter(filePath);
        TotalVisitor += amount;
        int jsonData = TotalVisitor;
        writer.Write(JsonConvert.SerializeObject(jsonData));
        writer.Close();
    }
}