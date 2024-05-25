namespace ReservationSystem;

public class Program
{
    public static IWorld World = new RealWorld();
    public static void Main()
    {
        Guide.LoadGuides();
        Tour.InitializeTours();
        Reservation.ValidateCodeAndShowMenu();

        // Guide guide = new (Guid.NewGuid(),"John Doe", "11");
        // GuideLoginMenu.ShowGuideMenu(guide);
    }
}
