namespace ReservationSystem;


public class JSON_Exception : View
{
    public void Show(Exception ex, string JSON_Title = "JSON")
    {
        WriteLine($"Failed to load {JSON_Title}: {ex.Message}");
    }
}