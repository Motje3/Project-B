namespace ReservationSystem;

public class GuideHasAlreadyCheckedInVisitor : View
{
    public static void Show(string ticket)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Write("Visitor with ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{ticket}");
        Console.ForegroundColor = ConsoleColor.Red;
        Write(" ticket has already been added.\n");
        Console.ResetColor();
        SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.WrongInput);
    }
}