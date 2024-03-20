public static class AllAvaibleScreens
{
    public static Screen StartingScreen;
    static AllAvaibleScreens()
    {
        StartingScreen = _makeStartingScreen();
    }

    private static ScreenStarting _makeStartingScreen()
    {
        List<string> WelcomeString = new()
        {
            "+---------------------------------+",
            "|                                 |",
            "|      Welcome to the museum      |",
            "|                                 |",
            "|     Please scan your ticket     |",
            "|      to register, cancel or     |",
            "|    edit your tour reservation   |",
            "|                                 |",
            "+---------------------------------+"
        };
        StaticConsoleObject welcomeBox = new(0, 0, WelcomeString);
        List<StaticConsoleObject> StaticObjects = new() { welcomeBox };

        List<string> entryTicketField = new() { "keyboard", "1", "1", "11" };
        SelectionConsoleObject entryTicketKeyboard = new(12, 10, entryTicketField);
        List<SelectionConsoleObject> Row0 = new() { null, null, null };
        List<SelectionConsoleObject> Row1 = new() { null, entryTicketKeyboard, null };
        List<SelectionConsoleObject> Row2 = new() { null, null, null };

        List<List<SelectionConsoleObject>> SelectionGrid = new() { Row0, Row1, Row2 };
        return new(StaticObjects,SelectionGrid,1,1,null,null);
    }
}