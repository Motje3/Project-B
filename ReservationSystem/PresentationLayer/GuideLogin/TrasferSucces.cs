namespace ReservationSystem;

public class TransferSucces : View
{
    public static void Show(Visitor visitor, Tour tourDetail)
    {
        Write($"Visitor with ticket ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{visitor.TicketCode}");
        Console.ResetColor();

        Write(" has been succesfully added to the next tour that starts at: ");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write($"{tourDetail.StartTime.ToString("HH:mm")}\n\n");
        Console.ResetColor();
        SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.ChceckIn);
    }
}