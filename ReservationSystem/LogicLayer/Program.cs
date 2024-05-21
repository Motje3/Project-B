namespace ReservationSystem;

public class Program
{
    public static IWorld World = new RealWorld();
    public static void Main()
    {
        // Guide.LoadGuides();
        // Tour.InitializeTours();
        // Reservation.ValidateCodeAndShowMenu();

        // public Guide(string name, Guid guideId, string password)
        // {
        //     GuideId = guideId;
        //     Name = name;
        //     Password = password;
        //     AllGuides.Add(this);
        // }
        Guide Alica = new Guide("Alice Johnson", Guid.NewGuid(), "222");
        Alica.AddVisitorLastMinute();
    }
}
