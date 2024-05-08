public static class AdminBackEnd
{
    public static void ChangeTourCapacity()
    {
        throw new NotImplementedException();
    }

    public static void ChangeTourTime()
    {
        throw new NotImplementedException();
    }

    public static string ReadPassword()
    {
        string password = "";
        while (true)
        {
            ConsoleKeyInfo info = Console.ReadKey(true);
            if (info.Key == ConsoleKey.Enter)
            {
                break;
            }
            else if (info.Key == ConsoleKey.Backspace)
            {
                if (password.Length > 0)
                {
                    password = password.Substring(0, password.Length - 1);
                    Console.Write("\b \b"); // Moves the cursor back, writes a space to erase the asterisk, then moves back again
                }
            }
            else
            {
                password += info.KeyChar;
                Console.Write("*");
            }
        }
        return password;
    }
}