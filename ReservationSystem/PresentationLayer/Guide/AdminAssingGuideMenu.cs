namespace ReservationSystem;

public class AdminAssingGuideMenu : View
{
   public static void ShowStartMess()
    {
        WriteLine("Select a guide to assign:");
    }
    public static void ShowSelectMess()
    {
        Write("Enter the number of the guide to assign: ");
    }
    public static void ShowAvailbleGuide(int i, Guide guide)
    {
        WriteLine($"{i + 1}. {Guide.AllGuides[i].Name}");
    }
}