namespace ReservationSystem;

public class GidsMainMenu : View
{
    public string Show()
    {
        WriteLine("What would you like to do?\n");
        WriteLine("1. See personal tours");
        WriteLine("2. Check in attending visitors for your next tour");
        WriteLine("3. Log out");
        return ReadLine();
    }
}