namespace ReservationSystem;


public class JSON_NotFound : View
{
    public static void Show(string JSON_Title = "JSON") 
    { 
        WriteLine($"{JSON_Title} file not found.".Trim());
        // Trim ensures that if no JSON_Title, to remove whitespace  
    }
}