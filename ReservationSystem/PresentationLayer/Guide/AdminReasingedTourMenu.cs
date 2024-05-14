namespace ReservationSystem;

public class ReasingedTourMenu : View
{
    public static void ShowStartMess()
    {
        WriteLine("Select a tour to reassign a guide:");
    }
    public static void ShowSelectMess()
    {
        Write("Enter the number of the tour to reassign: ");
    }
    public static void ShowAvailbleTour(int i, Tour tour)
    {
        WriteLine($"{i + 1}. Tour at {Tour.TodaysTours[i].StartTime} currently assigned to {Tour.TodaysTours[i].AssignedGuide?.Name ?? "No Guide"}");
    }
}