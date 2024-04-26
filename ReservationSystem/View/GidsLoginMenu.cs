namespace ReservationSystem;

public class GidsLoginMenu : View
{
    public string Show()
    {
        WriteLine("\nGuide login");
        WriteLine("Enter password: ");
        return Console.ReadLine();
    }
}

