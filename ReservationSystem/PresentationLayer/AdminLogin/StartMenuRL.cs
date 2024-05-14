namespace ReservationSystem;

public class StartMenuRL : View
{
    public static string Show()
    {
        WriteLine("\nAdmin Menu:");
        WriteLine("1. Assign a Different Guide to today's tours");
        WriteLine("2. Change the default guide's roaster (To be implemented)");

        WriteLine("4. Exit");

        WriteLine("5. Exit");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}