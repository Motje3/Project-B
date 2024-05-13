namespace ReservationSystem;

public class Program
{
    public static IWorld World = new RealWorld();  // this Mendentory for testing DONT REMOVE THIS!
    public static void Main()
    {
        Guide.LoadGuides();
        Tour.InitializeTours();
        ReservationPresentation.ValidateCodeAndShowMenu();
    }
}
