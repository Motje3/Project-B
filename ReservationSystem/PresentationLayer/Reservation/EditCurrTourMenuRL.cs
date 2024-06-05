namespace ReservationSystem;

public class EditCurrTourMenuRL : View
{
    public static string Show(string reservationDetails)
    {
        WriteLine(reservationDetails);
        Write("\nPlease choose an option by entering the ");
        ColourText.WriteColored("", "number ", ConsoleColor.Cyan, "left of the desired action\n");
        WriteLine("");


        // Save the current console color
        ConsoleColor originalColor = Console.ForegroundColor;

        // Change the color to cyan for the numbers
        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("1 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        WriteLine("Change my reservation time");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("2 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        WriteLine("Cancel my tour reservation");

        Console.ForegroundColor = ConsoleColor.Cyan;
        Write("3 | ");
        Console.ForegroundColor = originalColor; // Reset to the original color
        WriteLine("Return to Ticket Scanner");

        Write("\nEnter your choice: ");
        return ReadLine();  // choice
    }
}