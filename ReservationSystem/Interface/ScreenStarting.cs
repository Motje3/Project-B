
public class ScreenStarting : Screen
{
    public ScreenStarting(List<StaticConsoleObject> boxs, List<List<SelectionConsoleObject>> selectionGrid, int startingXposition, int startingYposition) : base(boxs, selectionGrid, startingXposition, startingYposition)
    {
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
            bool validCodeScanned = Ticket.IsCodeValid(currentKeyboard.text);
            bool maxLenCodeReached = currentKeyboard.text.Length == currentKeyboard.MaxLenKeyboard;

            // Trying to go to the next screen
            if (_enterPressed)
            {
                WrongCodeMessage.Reset();
                CodeNotLongEnoughMessage.Reset();

                if (maxLenCodeReached)
                {
                    if (validCodeScanned)
                    {
                        bool adminCodeScanned = currentKeyboard.text == "IKHOUVANART";
                        if (adminCodeScanned)
                        {
                            // To be implemented
                            break;
                        }
                        // Visitor code scanned
                        else
                        {
                            NextScreen[0].Show();
                        }
                    }
                    // Scanned code is not valid
                    else
                    {
                        WrongCodeMessage.Display();
                    }
                }
                // Scanned code not long enough
                else
                {
                    CodeNotLongEnoughMessage.Display();
                }
            }

            // Removing text from the currently selected keyboard
            if (_backspacePressed)
            {
                if (canDeleteText)
                {
                    currentKeyboard.text = currentKeyboard.text.Remove(currentKeyboard.text.Length - 1);
                }
            }

            // Adding text to the currently selected keyboard
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