using System;

public class ReservationManager
{
    private EntreeCodeValidator validator;
    private RondleidingReservation rondleiding;

    public ReservationManager(string jsonFilePath)
    {
        validator = new EntreeCodeValidator(jsonFilePath);
        rondleiding = new RondleidingReservation();
    }

    public void ValidateCodeAndProcessReservations()
    {
        bool isValidCode = false;
        while (!isValidCode)
        {
            Console.WriteLine("Enter your unique code:");
            string userCode = Console.ReadLine();

            if (validator.IsCodeValid(userCode))
            {
                Console.WriteLine("Welcome, your reservation is confirmed!");
                isValidCode = true;

                Console.WriteLine("Would you like to make a reservation for a rondleiding? (yes/no)");
                string response = Console.ReadLine();

                if (response.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    rondleiding.PromptForReservation();
                }
            }
            else
            {
                Console.WriteLine("Sorry, you do not have a reservation. Please try again.");
            }
        }
    }
}
