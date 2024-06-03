namespace ReservationSystem;

public class InvalidTicketCode : View
{
    public static void Show(string ticketCode)
    {
        try { Console.Clear(); } catch { }
        Console.ForegroundColor = ConsoleColor.Red;
        Write("The code '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{ticketCode}");
        Console.ForegroundColor = ConsoleColor.Red;
        Write("' is invalid.\n");
        Write("Please scan a valid ticket, or type '");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("Start");
        Console.ForegroundColor = ConsoleColor.Red;
        Write("' in to go back to previous menu\n\n");
        Console.ResetColor();
        SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.WrongInput);
    }
}