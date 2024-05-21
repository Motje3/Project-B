namespace ReservationSystem;

public class Program
{
    public static IWorld World = new RealWorld();
    public static void Main()
    {
        Guide.LoadGuides();
        Tour.InitializeTours();
        Reservation.ValidateCodeAndShowMenu();

        // Guide Alica = new Guide("Alice Johnson", Guid.NewGuid(), "222");
        // Visitor Jonh = new Visitor("444");
        // Alica.AddVisitorLastMinute(Jonh);
    }
}
