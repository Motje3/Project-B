using ReservationSystem;

public class GuideScannedInvalidTicket : View
{
    public static void Show()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Write("Invalid ticket.");
        Write($"Please, write '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"Start");
        Console.ForegroundColor = ConsoleColor.Red;
        Write("' to start the tour. Or '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("Q");
        Console.ForegroundColor = ConsoleColor.Red;
        Write("' to go back to previous menu");
        WriteLine("");
        WriteLine("");
        Console.ResetColor();
        SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.WrongInput);
    }
}