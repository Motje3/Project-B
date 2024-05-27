namespace ReservationSystem;

public class TransferSucces : View
{
    public static void Show(Tour tourDetail)
    {
        WriteLine($"Visitor succesfully added to the next tour that starts at: {tourDetail.StartTime.ToString("yyyy-MM-dd HH:mm:ss")} ");
    }
}