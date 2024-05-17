namespace ReservationSystem;

public class AccessPassed : View
{
    public static void Show(Guide g)
    {
        WriteLine($"\nWelcome {g.Name}\n");
    }
    public static void WelcomeAdmin()
    {
        WriteLine($"\nWelcome Admin\n");
    }
}