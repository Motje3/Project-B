namespace ReservationSystem;


public class JSON_Exception : View
{
    public static void Show(Exception ex, string JSON_Title = "JSON")
    {
        WriteLine($"Failed to load {JSON_Title} do to an error: {ex.Message}");
    }
}