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
        ConsoleKey info = Program.World.ReadKey(true).Key;
        while (info != ConsoleKey.Enter)
        {
            if (info != ConsoleKey.Backspace)
            {
                PasswordChar.Show();
                password += info.ToString();
            }
            else if (info == ConsoleKey.Backspace && password.Length > 0)
            {
                Program.World.Write("\b \b"); // Moves the cursor back, writes a space to erase the star, and moves back again.
                password = password[..^1]; // Removes the last character from the password string
            }
            info = Program.World.ReadKey(true).Key;
        }
        Space.Show(); // Console.WriteLine(); // Ensure the cursor moves to the next line after Enter is pressed
        return password;
    }

}