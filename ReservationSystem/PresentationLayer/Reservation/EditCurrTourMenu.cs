namespace ReservationSystem;

public class EditCurrTourMenu : View
{
    public static string Show(string reservationDetails)
    {
        WriteLine(reservationDetails);
        WriteLine("\nPlease choose an option:");
        WriteLine("1. Change my reservation time");
        WriteLine("2. Cancel my tour reservation");
        WriteLine("3. Return to main menu");
        Write("\nEnter your choice: ");
        return ReadLine();  // choice
    }
}