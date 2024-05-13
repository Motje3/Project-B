namespace ReservationSystem;

public class PasswordChar : View
{
    public static void Show(string sym = "*")
    {
        Write(sym[0].ToString());  // only 1 symbol can be used for hiding the Password 
    }
}