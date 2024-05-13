using ReservationSystem;

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
        PasswordChar PC = new PasswordChar();
        Space space = new Space();

        string password = "";
        ConsoleKeyInfo info = Console.ReadKey(true);
        while (info.Key != ConsoleKey.Enter)
        {
            if (info.Key != ConsoleKey.Backspace)
            {
                PC.Show();
                password += info.KeyChar;
            }
            else if (info.Key == ConsoleKey.Backspace && password.Length > 0)
            {
                Console.Write("\b \b"); // Moves the cursor back, writes a space to erase the star, and moves back again.
                password = password[..^1]; // Removes the last character from the password string
            }
            info = Console.ReadKey(true);
        }
        space.Show(); // Console.WriteLine(); // Ensure the cursor moves to the next line after Enter is pressed
        return password;
    }

}