public class Program
{
    public static void Main()

    {
        Guide.LoadGuides();
        Tour.InitializeTours();
        ReservationPresentation.ValidateCodeAndShowMenu();
    }
}
