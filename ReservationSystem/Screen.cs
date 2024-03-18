
using System.Diagnostics;
using System.Runtime.InteropServices;
public class Screen
{
    private ConsoleKey Key;
    private List<List<KeyboardConsoleObject>> _keyboardObjects;
    private List<List<SelectionConsoleObject>> _selectionObjectsGrid;
    private List<StaticConsoleObject> _staticObjects;
    private List<Screen> NextScreen;
    private Screen LastScreen;
    public (int, int) CurrentSelection;
    private int _currXpos;
    private int _currYpos;
    private SelectionConsoleObject _currentlySelectedObject;
    public bool canGoLeft;
    public bool canGoRight;
    public bool canGoUp;
    public bool canGoDown;
    private bool _leftPressed;
    private bool _rightPressed;
    private bool _downPressed;
    private bool _upPressed;
    private bool _ESCpressed;
    private bool _enterPressed;
    private bool _backspacePressed;
    private bool _alphaKeyboardPressed;
    private bool _numericalKeyboardPressed;
    private bool _keyboardSelected;

    // Each screen gets : 
    //   a list of static boxs (text that isn't going to change or move)
    //   a list of lists with selection boxs ()
    //   an int horizontale(x) position where selection begins
    //   an int vertical(y)  position where selection begins
    //   a list of Screen object nextScreen which is all screens to which you can go from this scren
    public Screen(List<StaticConsoleObject> boxs, List<List<SelectionConsoleObject>> selectionGrid, int startingXposition, int startingYposition, List<Screen> nextScreens, Screen lastScreen)
    {
        _staticObjects = boxs;
        _selectionObjectsGrid = selectionGrid;
        CurrentSelection = (startingXposition, startingYposition);
        _keyboardObjects = new();
        _addKeyboards();
        NextScreen = nextScreens;
        LastScreen = lastScreen;
    }

    public void GoNext(int positionInNextList)
    {
        NextScreen[positionInNextList].show();
    }

    public void GoBackwards()
    {
        LastScreen.show();
    }

    // Main loop function of a Screen object
    public virtual void show()
    {
        // call this method at the start of every overwritten show() method
        _showSetup();

        while (true)
        {
            //
            Key = Console.ReadKey(true).Key;
            _updateAll(Key);
            _keyboardSelected = _checkIfKeyboard(_currentlySelectedObject);
            // 

            if (_ESCpressed)
                break;

            if (_enterPressed && (_currXpos == 2 && _currYpos == 4))
            {
                break;
            }

            if (_leftPressed)
            {
                if (canGoLeft)
                {
                    CurrentSelection = (_currYpos, _currXpos - 1);
                }
            }
            if (_rightPressed)
            {
                if (canGoRight)
                {
                    CurrentSelection = (_currYpos, _currXpos + 1);
                }
            }
            if (_upPressed)
            {
                if (canGoUp)
                {
                    CurrentSelection = (_currYpos - 1, _currXpos);
                }
            }
            if (_downPressed)
            {
                if (canGoDown)
                {
                    CurrentSelection = (_currYpos + 1, _currXpos);
                }
            }

            // Check for keyboard selection
            if (_keyboardSelected)
            {
                var currentKeyboard = _keyboardObjects[_currYpos][_currXpos];
                bool canDeleteText = currentKeyboard.text.Length > 0;
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
            }
            DisplayAll();
        }
    }

    // Method which updates which buttons are currently pressed
    private void _updatePressedButtons(ConsoleKey key)
    {
        _leftPressed = key == ConsoleKey.LeftArrow;
        _rightPressed = key == ConsoleKey.RightArrow;
        _upPressed = key == ConsoleKey.UpArrow;
        _downPressed = key == ConsoleKey.DownArrow;
        _ESCpressed = key == ConsoleKey.Escape;
        _enterPressed = key == ConsoleKey.Enter;
        _backspacePressed = key == ConsoleKey.Backspace;
        _alphaKeyboardPressed = key.ToString().Length == 1;
        _numericalKeyboardPressed = _isNumercialKeyboard(key);
    }
    // Method which is supposed to be used as first in the show() method
    private void _showSetup()
    {
        Console.Clear();
        _maximizeWindow();
        Console.CursorVisible = false;
    }

    // Method which updates directions where you are allowed to go
    private void _updateDirections()
    {
        int currXpos = CurrentSelection.Item2;
        int currYpos = CurrentSelection.Item1;
        canGoDown = _selectionObjectsGrid[currYpos + 1][currXpos] != null;
        canGoUp = _selectionObjectsGrid[currYpos - 1][currXpos] != null;
        canGoLeft = _selectionObjectsGrid[currYpos][currXpos - 1] != null;
        canGoRight = _selectionObjectsGrid[currYpos][currXpos + 1] != null;
    }

    // Method which updates all fields which change when you move selection
    private void _updateSelections()
    {
        _currXpos = CurrentSelection.Item2;
        _currYpos = CurrentSelection.Item1;
        _currentlySelectedObject = _selectionObjectsGrid[_currYpos][_currXpos];
    }

    // Method which uses all  _updateXYX functions
    private void _updateAll(ConsoleKey key)
    {
        _updateSelections();
        _updateDirections();
        _updatePressedButtons(Key);
    }

    // Method for printing all objects that are supposed to be printed
    public void DisplayAll()
    {
        // Print all objects that are not going to change
        for (int Index = 0; Index < _staticObjects.Count; Index++)
        {
            StaticConsoleObject currBox = _staticObjects[Index];
            currBox.Display();
        }
        // Print all selection objects 
        for (int rowIndex = 0; rowIndex < _selectionObjectsGrid.Count; rowIndex++)
        {
            var currentRow = _selectionObjectsGrid[rowIndex];
            for (int columnIndex = 0; columnIndex < currentRow.Count; columnIndex++)
            {
                var currentColumn = currentRow[columnIndex];
                // skip column if null
                if (currentColumn == null)
                    continue;

                if (CurrentSelection == (rowIndex, columnIndex))
                {
                    // Is a normal SelectionConsoleObject
                    if (!_checkIfKeyboard(currentColumn))
                    {
                        currentColumn.Selected = true;
                        currentColumn.Display();
                        currentColumn.Selected = false;
                    }
                    // Is a KeyboardConsoleObject
                    else
                    {
                        KeyboardConsoleObject selectedKeyboard = _keyboardObjects[rowIndex][columnIndex];
                        selectedKeyboard.Selected = true;
                        selectedKeyboard.Display();
                        selectedKeyboard.Selected = false;
                    }
                }
                else
                {
                    // Next line is to make sure that it doesn't print as selected when not selected
                    if (!_checkIfKeyboard(currentColumn))
                    {
                        currentColumn.Selected = false;
                        currentColumn.Display();
                    }
                    // Is a KeyboardConsoleObject
                    else
                    {
                        KeyboardConsoleObject selectedKeyboard = _keyboardObjects[rowIndex][columnIndex];
                        selectedKeyboard.Selected = false;
                        selectedKeyboard.Display();
                    }
                }
            }
        }
    }

    private bool _checkIfKeyboard(SelectionConsoleObject obj)
    {
        if (obj._multiLineString[0] == "keyboard")
            return true;
        else
            return false;
    }

    protected KeyboardConsoleObject _makeKeyboardObject(SelectionConsoleObject obj)
    {
        int gridPosX = int.Parse(obj._multiLineString[1]);
        int gridPosY = int.Parse(obj._multiLineString[2]);
        int maxLen = int.Parse(obj._multiLineString[3]);
        return new(obj.PositionX, obj.PositionY, gridPosX, gridPosY, maxLen);
    }

    public bool _isNumercialKeyboard(ConsoleKey key)
    {
        try
        {
            string formattedKey = $"{key.ToString()[1]}";
            int possibleInt = int.Parse(formattedKey);
            return true;
        }
        catch (FormatException)
        {
            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    // To be used ONLY in the constructor
    protected void _addKeyboards()
    {
        for (int rowIndex = 0; rowIndex < _selectionObjectsGrid.Count; rowIndex++)
        {
            var currentRow = _selectionObjectsGrid[rowIndex];
            _keyboardObjects.Add(new() { });
            for (int columnIndex = 0; columnIndex < currentRow.Count; columnIndex++)
            {
                _keyboardObjects[rowIndex].Add(null);
            }
        }

        for (int rowIndex = 0; rowIndex < _selectionObjectsGrid.Count; rowIndex++)
        {
            var currentRow = _selectionObjectsGrid[rowIndex];
            for (int columnIndex = 0; columnIndex < currentRow.Count; columnIndex++)
            {
                var currentColumn = currentRow[columnIndex];
                bool isCurrentlyNull = _selectionObjectsGrid[rowIndex][columnIndex] == null;
                bool isAKeyboard;
                if (!isCurrentlyNull)
                {
                    isAKeyboard = _selectionObjectsGrid[rowIndex][columnIndex]._multiLineString[0] == "keyboard";
                }
                else
                {
                    isAKeyboard = false;
                }
                if (isAKeyboard)
                {
                    KeyboardConsoleObject keyBoardToBeAdded = _makeKeyboardObject(currentColumn);
                    _keyboardObjects[rowIndex][columnIndex] = keyBoardToBeAdded;
                }

            }
        }
    }

    // Method which automatically maximizes window and prevents it from resizing
    private void _maximizeWindow()
    {
        [DllImport("user32.dll")]
        static extern bool ShowWindow(System.IntPtr hWnd, int cmdShow);
        Process p = Process.GetCurrentProcess();
        ShowWindow(p.MainWindowHandle, 3); //SW_MAXIMIZE = 3

        const int MF_BYCOMMAND = 0x00000000;
        const int SC_SIZE = 0xF000;
        const int SC_MOVE = 0xF010;
        const int SC_DEFAULT = 0xF160;
        const int SC_RESTORE = 0xF120;
        [DllImport("user32.dll")]
        static extern int DeleteMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("user32.dll")]
        static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("kernel32.dll", ExactSpelling = true)]
        static extern IntPtr GetConsoleWindow();
        IntPtr handle = GetConsoleWindow();
        IntPtr sysMenu = GetSystemMenu(handle, false);
        if (handle != IntPtr.Zero)
        {
            DeleteMenu(sysMenu, SC_SIZE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_MOVE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_RESTORE, MF_BYCOMMAND);
            DeleteMenu(sysMenu, SC_DEFAULT, MF_BYCOMMAND);
        }
    }
}