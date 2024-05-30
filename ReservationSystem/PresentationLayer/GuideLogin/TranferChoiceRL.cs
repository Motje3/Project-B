namespace ReservationSystem;

public class TransferChoiceRL : View
{
    public static void ShowMessage()
    {
        WriteLine("\nVistor already has a reservation,");
        WriteLine("Would you like to transfer their reservation? ");
    }

    public static void ShowChoice()
    {
        WriteLine("\n1 | Yes");
        WriteLine("2 | No");

        Write("\nEnter your choice: ");
    }
}


