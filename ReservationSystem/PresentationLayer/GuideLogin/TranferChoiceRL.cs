namespace ReservationSystem;

public class TransferChoiceRL : View
{
    public static void ShowMessage()
    {
        WriteLine("\nVistor is already registerd for a tour,");
        WriteLine("Would you like to transfer the tour? ");
    }

    public static string ShowChoice()
    {
        WriteLine("\n1 | Yes");
        WriteLine("2 | No");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}


