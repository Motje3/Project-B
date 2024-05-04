namespace ReservationSystem;

public class CheckIn : View
{
    public void Show(string Code)
    {
        WriteLine($"Currently checked in tickets: [{Code}]");
    }
}


