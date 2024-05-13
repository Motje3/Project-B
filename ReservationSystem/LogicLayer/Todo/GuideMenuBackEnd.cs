using Newtonsoft.Json;
using ReservationSystem;

public static class GuideMenuBackEnd
{
    public class Credential
    {
        public string Password { get; set; }
    }

    
    public static bool AuthenticateGuide(string password)
    {
        try
        {
            List<Credential> credentials = LoadUserCredentials();
            return credentials.Any(cred => cred.Password == password);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error authenticating guide: {ex.Message}");
            return false;
        }
    }


    private static List<Credential> LoadUserCredentials()
    {
        string filePath = "./JSON-Files/GidsCredentials.json";
        try
        {
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<List<Credential>>(jsonData) ?? new List<Credential>();
            }
            else
            {
                JSON_NotFound.Show("Credentials");  // JSON Title
                return new List<Credential>();
            }
        }
        catch (Exception ex)
        {
            JSON_Exception.Show(ex, "Credentials");  // JSON Title
            return new List<Credential>();
        }
    }
}