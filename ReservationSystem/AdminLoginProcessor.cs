using Newtonsoft.Json;

public class AdminLoginProcessor
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public void ProcessLoginForm()
    {
        Console.WriteLine("\nAdmin login\n");

        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (AuthenticateUser(username, password))
        {
            Console.WriteLine("\nAccess Granted!\n");
            // Here you can add more admin-specific functionalities
        }
        else
        {
            Console.WriteLine("\nAccess Denied. Invalid username or password.\n");
        }
    }

    private bool AuthenticateUser(string username, string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Username == username && cred.Password == password);
    }

    private List<Credential> LoadUserCredentials()
    {
        string filePath = "./JSON-Files/AdminCredentials.json";
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<Credential>>(jsonData) ?? new List<Credential>();
        }
        else
        {
            Console.WriteLine("Credential file not found.");
            return new List<Credential>();
        }
    }
}
