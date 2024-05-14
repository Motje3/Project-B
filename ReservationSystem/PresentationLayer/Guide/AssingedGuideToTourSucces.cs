namespace ReservationSystem;

public class AssignedGuideToTourSucces : View
{
    public static void Show(int guideIndex, int tourIndex)
    {
        Console.WriteLine($"Guide {Guide.AllGuides[guideIndex].Name} has been successfully assigned to the tour at {Tour.TodaysTours[tourIndex].StartTime:hh:mm tt} o'clock.");
    }
}