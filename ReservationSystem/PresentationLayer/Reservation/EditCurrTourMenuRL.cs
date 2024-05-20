namespace ReservationSystem;

public class EditCurrTourMenuRL : View
{
    public static string Show(string reservationDetails)
    {
        WriteLine(reservationDetails);
        Console.Write("\nPlease choose an option by entering the ");
        ColourText.WriteColored("", "number ", ConsoleColor.Cyan, "left of the desired action\n");
        Program.World.WriteLine("");


        // Save the current console color
        ConsoleColor originalColor = Console.ForegroundColor;

        // Change the color to cyan for the numbers
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("1 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        Console.WriteLine("Change my reservation time");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("2 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        Console.WriteLine("Cancel my tour reservation");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("3 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        Console.WriteLine("Return to Main Menu");

        Write("\nEnter your choice: ");
        return ReadLine();  // choice
    }
}