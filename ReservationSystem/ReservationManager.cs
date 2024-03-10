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
                Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                isValidCode = true;

                string response = "";
                do
                {
                    Console.WriteLine("Please choose an option:\n\n1. Make a reservation for a guided tour \n2. Edit an existing reservation \n3. Cancel a reservation");
                    response = Console.ReadLine()?.Trim(); // Safely trim the response

                    switch (response)
                    {
                        case "1":
                            Visitor visitorWithReservation = guidedTour.PromptForReservation();
                            if (visitorWithReservation != null && visitorWithReservation.Tickets.Any())
                            {
                                ticketStorageManager.SaveTicketInfo(visitorWithReservation);
                            }
                            else
                            {
                                Console.WriteLine("Unfortunately, we could not process your reservation at this time.");
                            }
                            break;
                        case "2":
                            // Implement the logic to edit a reservation
                            EditReservation();
                            break;
                        case "3":
                            // Implement the logic to cancel a reservation
                            CancelReservation();
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            response = ""; // Ensure the loop continues on invalid input
                            break;
                    }
                } while (response != "1" && response != "2" && response != "3");
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }

    public void EditReservation()
    {
        Console.WriteLine("\nEnter your ticket code:");
        string ticketCode = Console.ReadLine().Trim();

        List<Ticket> ticketInfoList = ticketStorageManager.LoadTicketInfo();
        var ticketToEdit = ticketInfoList.FirstOrDefault(ticket => ((dynamic)ticket).TicketCode == ticketCode);

        if (ticketToEdit != null)
        {
            Console.WriteLine("\nCurrent reservation time: " + ((dynamic)ticketToEdit).Time);
            Console.WriteLine("Enter the new time you would like to reserve (between 09:00 and 17:00):");
            string newTime = Console.ReadLine();

            // Validate the new time format and range
            if (guidedTour.ValidateTimeFormat(newTime) && guidedTour.IsTimeInRange(newTime))
            {
                // Update the ticket time
                ((dynamic)ticketToEdit).Time = newTime;
                // Save the updated ticket info back to the JSON file
                ticketStorageManager.SaveTicketInfoList(ticketInfoList);
                Console.WriteLine("\nYour reservation has been updated to the new time: " + newTime);
            }
            else
            {
                Console.WriteLine("\nInvalid time. The time should be in HH:MM format and between 09:00 and 17:00.");
            }
        }
        else
        {
            Console.WriteLine("\nTicket code not found. Please enter a valid ticket code.");
        }
    }


    private void CancelReservation()
    {
        Console.WriteLine("\nEnter the ticket code of the reservation you wish to cancel:");

        string ticketCode = Console.ReadLine().Trim();

        List<Ticket> ticketInfoList = ticketStorageManager.LoadTicketInfo();

        var ticketToCancel = ticketInfoList.FirstOrDefault(ticket =>
            string.Equals(ticket.TicketCode, ticketCode, StringComparison.OrdinalIgnoreCase));

        if (ticketToCancel != null && ticketToCancel.IsActive)
        {
            ticketToCancel.IsActive = false;
            ticketStorageManager.SaveTicketInfoList(ticketInfoList); // Make sure this method saves the list with IsActive info
            Console.WriteLine("\nYour reservation has been canceled.");
        }
        else
        {
            Console.WriteLine("\nTicket code not found or reservation already canceled.");
        }
    }
}
