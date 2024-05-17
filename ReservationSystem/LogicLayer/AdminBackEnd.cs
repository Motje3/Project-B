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

        string password = "";
        ConsoleKeyInfo info = Program.World.ReadKey(true);
        ConsoleKey pressed = info.Key;
        char chosen = info.KeyChar;
        
        while (pressed != ConsoleKey.Enter)
        {
            if (pressed != ConsoleKey.Backspace)
            {
                PasswordChar.Show();
                password += chosen;
            }
            else if (pressed == ConsoleKey.Backspace && password.Length > 0)
            {
                Program.World.Write("\b \b"); // Moves the cursor back, writes a space to erase the star, and moves back again.
                password = password[..^1]; // Removes the last character from the password string
            }
            info = Program.World.ReadKey(true);
            pressed = info.Key;
            chosen = info.KeyChar;
        }
        Space.Show(); // Console.WriteLine(); // Ensure the cursor moves to the next line after Enter is pressed
        return password;
    }

}