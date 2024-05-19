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
}
