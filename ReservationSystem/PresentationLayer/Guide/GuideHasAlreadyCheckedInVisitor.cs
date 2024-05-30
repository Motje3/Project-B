namespace ReservationSystem;

public class GuideHasAlreadyCheckedInVisitor : View
{
    public static void Show()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        WriteLine("Visitor has already been added.\n");
        Console.ResetColor();
        SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.WrongInput);
    }
}