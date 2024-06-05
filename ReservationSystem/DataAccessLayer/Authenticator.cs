using ReservationSystem;
public static class Authenticator
{
    public class Credential
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public static bool AuthenticateUser(string password)
    {
        List<Credential> credentials = LoadUserCredentials();
        return credentials.Any(cred => cred.Password == password);
    }

    public static List<Credential> LoadUserCredentials()
    {
        string filePath = "./JSON-Files/AdminCredentials.json";
        if (File.Exists(filePath))
        {
            return JsonHelper.LoadFromJson<List<Credential>>(filePath);
        }
        else
        {
            Program.World.WriteLine("Credential file not found.");
            return new List<Credential>();
        }
    }
}
