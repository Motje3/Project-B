// This class is meant for text that can be selected but cannot be typed in
public class SelectionConsoleObject : StaticConsoleObject
{
    public bool Selected;
    public SelectionConsoleObject(int positionX, int positionY, List<string> text, ConsoleColor textColor = ConsoleColor.Blue, ConsoleColor backgroundColor = ConsoleColor.Black) : base(positionX, positionY, text, textColor, backgroundColor)
    {
        Selected = false;
    }

    public override void Display()
    {
        if (Selected)
        {
            Console.ForegroundColor = _textColor;
            Console.BackgroundColor = ConsoleColor.White;
            int offset = 0;
            foreach (string currentString in _multiLineString)
            {
                Console.SetCursorPosition(PositionX, PositionY + offset);
                Console.Write(currentString);
                offset += 1;
            }
            Console.ResetColor();
        }
        else
            base.Display();
    }
    public static string TimesString(int howMany, char toMultiply)
    {
        string ret = "";
        for (int i = 0; i < howMany; i++)
        {
            ret += toMultiply;
        }
        return ret;
    }
}