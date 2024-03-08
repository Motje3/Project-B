using System;

public class ReservationManager
{
    private EntreeCodeValidator validator;
    private GuidedTour guidedTour;

    public ReservationManager()
    {
        validator = new EntreeCodeValidator();
        guidedTour = new GuidedTour();
    }

    public void ValidateCodeAndProcessReservations()
    {
        bool isValidCode = false;
        while (!isValidCode)
        {
            Console.WriteLine("Enter your unique ticket code:");
            string userCode = Console.ReadLine();

            if (validator.IsCodeValid(userCode))
            {
                Console.WriteLine("Welcome, your ticket is confirmed!");
                isValidCode = true;

                Console.WriteLine("Would you like to make a reservation for a guided tour? (yes/no)");
                string? response = null;
                do
                {
                    if (response != null)
                        Console.WriteLine("Wrong input try again");
                    response = Console.ReadLine();
                } while (!new List<string>() { "yes", "no" }.Contains(response));

                if (response.Equals("yes", StringComparison.OrdinalIgnoreCase))
                {
                    guidedTour.PromptForReservation();
                }
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }
}
