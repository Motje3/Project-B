namespace ReservationSystem;

public class ReservationCancelSucces : View
{
    public static void Show()
    {
        ColourText.WriteColored("Your reservation has been ", "cancelled  ", ConsoleColor.Cyan, "successfully\n");
    }
}