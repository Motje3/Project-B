// This Class is meant for text that is not going to be selected
public class StaticConsoleObject
{
    public int PositionX;
    public int PositionY;
    // Each element is a new line
    public List<string> _multiLineString { get; private set; }
    public ConsoleColor _textColor { get; private set; }
    public ConsoleColor _backgroundColor { get; private set; }
    public bool ToBeDisplayed;


    public StaticConsoleObject(int positionX, int positionY, List<string> text, ConsoleColor textColor = ConsoleColor.Blue, ConsoleColor backgroundColor = ConsoleColor.Black)
    {
        PositionX = positionX;
        PositionY = positionY;
        _multiLineString = text;
        _textColor = textColor;
        _backgroundColor = backgroundColor;
        ToBeDisplayed = true;
    }

    public virtual void Display()
    {
        Console.ForegroundColor = _textColor;
        Console.BackgroundColor = _backgroundColor;
        int offset = 0;
        foreach (string currentString in _multiLineString)
        {
            Console.SetCursorPosition(PositionX, PositionY + offset);
            Console.Write(currentString);
            offset += 1;
        }
        Console.ResetColor();
    }

    public void Reset()
    {
        Console.ForegroundColor = ConsoleColor.Black;
        // Go through each line in multilinestring
        for (int listIndex = 0; listIndex < _multiLineString.Count; listIndex++)
        {
            string currentString = _multiLineString[listIndex];

            for (int charIndex = 0; charIndex < currentString.Length; charIndex++)
            {
                Console.SetCursorPosition(PositionX + charIndex, PositionY + listIndex);
                Console.Write(" ");
            }
        }
    }
}