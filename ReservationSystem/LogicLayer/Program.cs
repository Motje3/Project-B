namespace ReservationSystem;
using Newtonsoft.Json;

public class Program
{
    public static IWorld World = new RealWorld();
    public static void Main()
    {
        Guide.LoadGuides();
        TourTools.InitializeTours();
        Reservation.ValidateCodeAndShowMenu();

        // Guide guide = new (Guid.NewGuid(),"John Doe", "11");
        // GuideLoginMenu.ShowGuideMenu(guide);
    }

    public static string DeserialeTours() 
    {
        return JsonConvert.SerializeObject(TourTools.TodaysTours);
    }
}
