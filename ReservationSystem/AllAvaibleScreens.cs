public static class AllAvaibleScreens
{
    public static Screen StartingScreen;
    static AllAvaibleScreens()
    {
        StartingScreen = _makeStartingScreen();
    }

    private static Screen _makeStartingScreen()
    {
        List<string> WelcomeString = new()
        {
            "+---------------------------------+",
            "|      Welcome to the museum      |",
            "|                                 |",
            "|     Please scan your ticket     |",
            "|      to register, cancel or     |",
            "|    edit your tour reservation   |",
            "+---------------------------------+"
        };
        StaticConsoleObject welcomeBox = new(0, 0, WelcomeString);

        List<string> entryTicketField = new() { "keyboard", "1", "1", "11" };
        SelectionConsoleObject entryTicketKeyboard = new()
        List<SelectionConsoleObject> Row1 = new() { };
        List<SelectionConsoleObject> Row2 = new() { };
        List<SelectionConsoleObject> Row3 = new() { };
    }
}