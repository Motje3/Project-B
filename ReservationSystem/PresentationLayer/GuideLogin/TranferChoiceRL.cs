namespace ReservationSystem;

public class TransferChoiceRL : View
{
    public static void ShowMessage()
    {
        WriteLine("Vistor is already registerd for a tour,");
        WriteLine("do you want to transfer the tour?");
    }

    public static string ShowChoice()
    {
        WriteLine("\n1 | Yes");
        WriteLine("2 | No");

        Write("\nEnter your choice: ");
        return ReadLine();
    }
}


