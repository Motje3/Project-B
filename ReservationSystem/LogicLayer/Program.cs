using System.Media;

namespace ReservationSystem;

public class Program
{
    public static IWorld World = new RealWorld();
    public static void Main()
    {
        Reservation.ValidateCodeAndShowMenu();
    }
}
