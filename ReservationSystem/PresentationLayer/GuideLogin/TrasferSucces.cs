namespace ReservationSystem;

public class TransferSucces : View
{
    public static void Show(Tour tourDetail)
    {
        WriteLine("\nVisitor succesfully added to the next tour that starts at: ");
        WriteLine(tourDetail.StartTime.ToString("yyyy-MM-dd HH:mm:ss"));  // WriteLine parameter must be a string
    }
}