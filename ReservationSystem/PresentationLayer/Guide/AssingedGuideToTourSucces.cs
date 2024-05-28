namespace ReservationSystem;

public class AssignedGuideToTourSucces : View
{
    public static void Show(int guideIndex, int tourIndex)
    {
        WriteLine($"Guide {Guide.AllGuides[guideIndex].Name} has been successfully assigned to the tour at {TourTools.TodaysTours[tourIndex].StartTime:hh:mm tt} o'clock.");
    }
}