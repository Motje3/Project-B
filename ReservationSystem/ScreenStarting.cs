
public class ScreenStarting : Screen
{
    EntreeCodeValidator validator;
    public ScreenStarting(List<StaticConsoleObject> boxs, List<List<SelectionConsoleObject>> selectionGrid, int startingXposition, int startingYposition, List<Screen> nextScreens, Screen lastScreen) : base(boxs, selectionGrid, startingXposition, startingYposition, nextScreens, lastScreen)
    {
        validator = new();
    }

    public override void Show()
    {
        _showSetup();

        while (true)
        {
            _UpdateAllAndReadUserInput();

            List<string> CodeNotLongEnoughText = new()
            {
                "---------------------------------------",
                " Scanned entry code is not long enough ",
                "           Please try again            ",
                "---------------------------------------",
            };
            StaticConsoleObject CodeNotLongEnoughMessage = new(0, 12, CodeNotLongEnoughText, ConsoleColor.Red);

            List<string> WrongCodeText = new()
            {
                "-----------------------------",
                " Wrong code has been scanned ",
                "      Please try again       ",
                "-----------------------------",
            };
            StaticConsoleObject WrongCodeMessage = new(4, 12, WrongCodeText, ConsoleColor.Red);

            var currentKeyboard = _keyboardObjects[_currYpos][_currXpos];
            bool canDeleteText = currentKeyboard.text.Length > 0;
            bool validCodeScanned = validator.IsCodeValid(currentKeyboard.text);
            bool maxLenCodeReached = currentKeyboard.text.Length == currentKeyboard.MaxLenKeyboard;

            if (_enterPressed)
            {
                WrongCodeMessage.Reset();
                CodeNotLongEnoughMessage.Reset();

                if (maxLenCodeReached)
                {
                    if (validCodeScanned)
                    {

                    }
                    else
                    {
                        WrongCodeMessage.Display();
                    }
                }
                else
                {
                    CodeNotLongEnoughMessage.Display();
                }
            }

            if (_backspacePressed)
            {
                if (canDeleteText)
                {
                    currentKeyboard.text = currentKeyboard.text.Remove(currentKeyboard.text.Length - 1);
                }
            }
            if ((_alphaKeyboardPressed || _numericalKeyboardPressed) && currentKeyboard.text.Length != currentKeyboard.MaxLenKeyboard)
            {
                if (_alphaKeyboardPressed)
                    currentKeyboard.text = currentKeyboard.text += Key.ToString();
                else if (_numericalKeyboardPressed)
                    currentKeyboard.text = currentKeyboard.text += Key.ToString()[1];
            }

            DisplayAll();
        }

    }
}