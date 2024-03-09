using System;

public class ReservationManager
{
    private EntreeCodeValidator validator;
    private GuidedTour guidedTour;
    private TicketStorageManager ticketStorageManager;

    public ReservationManager()
    {
        validator = new EntreeCodeValidator();
        guidedTour = new GuidedTour();
        ticketStorageManager = new TicketStorageManager("ticketsmanager.json");
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
                string response = "";
                do
                {
                    if (!string.IsNullOrEmpty(response))
                        Console.WriteLine("Wrong input, try again.");

                    response = Console.ReadLine()?.Trim().ToLower(); // Safely trim and lowercase the response
                } while (response != "yes" && response != "no"); // Simplified condition

                if (response.Equals("yes"))
                {
                    Visitor visitorWithReservation = guidedTour.PromptForReservation();
                    if (visitorWithReservation != null && visitorWithReservation.Tickets.Any())
                    {
                        ticketStorageManager.SaveTicketInfo(visitorWithReservation);
                        //Tickets saved to json bestand
                    }
                    else
                    {
                        Console.WriteLine("Unfortunately, we could not process your reservation at this time.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }

}
