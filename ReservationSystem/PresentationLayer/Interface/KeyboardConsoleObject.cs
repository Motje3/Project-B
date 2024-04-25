// this  is meant for making a 1-dimensional box that can be selected and typed in
public class KeyboardConsoleObject : SelectionConsoleObject
{
    public string text;
    public int MaxLenKeyboard;
    private int _timer;
    public int GridPosX;
    public int GridPosY;

    public KeyboardConsoleObject(int positionX, int positionY, int gridPosX, int gridPosY, int maxLenKeyboard, ConsoleColor textColor = ConsoleColor.Black, ConsoleColor backgroundColor = ConsoleColor.White) : base(positionX, positionY, new() { TimesString(maxLenKeyboard + 1, ' ') }, textColor, backgroundColor)
    {
        text = "";
        GridPosX = gridPosX;
        GridPosY = gridPosY;
        MaxLenKeyboard = maxLenKeyboard;
    }
    public override void Display()
    {
        if (Selected)
        {
            Console.ForegroundColor = _textColor;
            Console.BackgroundColor = ConsoleColor.Gray;
            foreach (string currentString in _multiLineString)
            {
                Console.SetCursorPosition(PositionX, PositionY);
                Console.Write(currentString);
            }
            Console.SetCursorPosition(PositionX, PositionY);

            for (int charIndex = 0; charIndex < text.Length; charIndex++)
            {
                if (charIndex != text.Length - 1)
                {
                    Console.ForegroundColor = _textColor;
                    Console.BackgroundColor = ConsoleColor.Gray;
                    char currentCharacter = text[charIndex];
                    Console.Write(currentCharacter);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = _textColor;
                    Console.BackgroundColor = ConsoleColor.White;
                    char currentCharacter = text[charIndex];
                    Console.Write(currentCharacter);
                    Console.ResetColor();
                }

            }
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = _textColor;
            Console.BackgroundColor = ConsoleColor.DarkGray;
            foreach (string currentString in _multiLineString)
            {
                Console.SetCursorPosition(PositionX, PositionY);
                Console.Write(currentString);
            }
            Console.SetCursorPosition(PositionX, PositionY);
            foreach (char character in text)
            {
                Console.Write(character);
            }
            Console.ResetColor();
        }
    }
}