using ReservationSystem;
public static class ColourText
{
    public static void WriteColoredLine(string beforeColor, string colorWord, ConsoleColor color, string afterColor = "")
    {
        Console.Write(beforeColor);
        Console.ForegroundColor = color;
        Console.Write(colorWord);
        Console.ResetColor();
        Console.WriteLine(afterColor);
    }

    public static void WriteColoredLine(string beforeColor, ConsoleColor color, string colorWord, string afterColor = "")
    {
        Console.Write(beforeColor);
        Console.ForegroundColor = color;
        Console.Write(colorWord);
        Console.ResetColor();
        Console.WriteLine(afterColor);
    }

    public static void WriteColored(string beforeColor, string colorWord, ConsoleColor color, string afterColor = "")
    {
        Program.World.Write(beforeColor);
        Console.ForegroundColor = color;
        Program.World.Write(colorWord);
        Console.ResetColor();
        Program.World.Write(afterColor);
    }

    public static string GetColoredString(string beforeColor, string colorWord, ConsoleColor color, string afterColor = "")
    {
        return beforeColor + GetColorCode(color) + colorWord + GetResetCode() + afterColor;
    }

    private static string GetColorCode(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => "\x1b[30m",
            ConsoleColor.DarkBlue => "\x1b[34m",
            ConsoleColor.DarkGreen => "\x1b[32m",
            ConsoleColor.DarkCyan => "\x1b[36m",
            ConsoleColor.DarkRed => "\x1b[31m",
            ConsoleColor.DarkMagenta => "\x1b[35m",
            ConsoleColor.DarkYellow => "\x1b[33m",
            ConsoleColor.Gray => "\x1b[37m",
            ConsoleColor.DarkGray => "\x1b[90m",
            ConsoleColor.Blue => "\x1b[94m",
            ConsoleColor.Green => "\x1b[92m",
            ConsoleColor.Cyan => "\x1b[96m",
            ConsoleColor.Red => "\x1b[91m",
            ConsoleColor.Magenta => "\x1b[95m",
            ConsoleColor.Yellow => "\x1b[93m",
            ConsoleColor.White => "\x1b[97m",
            _ => "\x1b[0m",
        };
    }

    private static string GetResetCode()
    {
        return "\x1b[0m";
    }
}
