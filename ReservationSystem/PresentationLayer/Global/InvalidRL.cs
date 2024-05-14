// this can be re-used for multiple classes
namespace ReservationSystem;

public class InvalidRL : View
{
    public static void Show(string subMessage = "Please enter a valid option.") 
    {
        WriteLine($"Invalid choice. {subMessage}");
    }
}