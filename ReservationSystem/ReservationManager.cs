using Newtonsoft.Json;
public class ReservationManager
{
    private EntreeCodeValidator validator;
    private GuidedTour guidedTour;
    private MenuManager menumanager;
    private List<Ticket> _tickets;

    public List<Ticket> Tickets
    {
        get { return _tickets; }
    }

    public ReservationManager()
    {
        validator = new EntreeCodeValidator();
        guidedTour = new GuidedTour();
        menumanager = new MenuManager(this, guidedTour); // Pass the same GuidedTour instance
        guidedTour.LoadToursFromFile("./JSON-Files/guidedTours.json");
        _tickets = Ticket.LoadTicketsFromFile("./JSON-Files/OnlineTickets.json");
    }

    public void ValidateCodeAndProcessReservations()
    {
        bool isValidCode = false;
        while (!isValidCode)
        {
            Console.WriteLine("Enter your unique ticket code:");
            string userCode = Console.ReadLine();

            if (userCode == "123")
            {
                AdminLoginProcessor adminManager = new AdminLoginProcessor();
                adminManager.ProcessLoginForm(guidedTour); // Pass the instance of GuidedTour here
                return; // Exit the method after admin login form is processed
            }
            else if (userCode == "456")
            {
                GidsLoginProcessor gidsManager = new GidsLoginProcessor();
                gidsManager.ProcessLoginForm();
                return; //Exit the method after guide login form is processed
            }
            else if (validator.IsCodeValid(userCode))
            {
                Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                isValidCode = true;

                if (VisitorAlreadyHasReservation(userCode))
                {
                    menumanager.ShowFullMenu(userCode);
                }
                else
                {
                    // Retrieve the ticket associated with the code
                    var ticket = validator.GetTicketByCode(userCode);
                    if (ticket != null)
                    {
                        // Assuming the ShowRestrictedMenu method will handle the logic for
                        // allowing all visitors in the ticket to join a tour.
                        menumanager.ShowRestrictedMenu(userCode);
                    }
                    else
                    {
                        Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }



    public bool EditReservation(string ticketCode, int newTourHour)
    {
        string reservationsFilePath = "./JSON-Files/reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(reservationsFilePath)) ?? new List<dynamic>();

        var reservation = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservation != null)
        {
            // Directly using TourHour, assuming it's always an integer.
            int oldHour = (int)reservation.TourHour;

            // Attempt to update the tour hour for the visitor.
            bool updateSuccess = guidedTour.UpdateVisitorTour(ticketCode, newTourHour);
            if (updateSuccess)
            {
                // Update the reservation details.
                reservation.TourHour = newTourHour;

                // Write the changes back to the reservations file.
                File.WriteAllText(reservationsFilePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));

                // Now, also update the guided tours file to reflect the change.
                guidedTour.SaveGuidedToursToFile(); // Assuming this method saves the entire state of `TourSlots` to a file.

                Console.WriteLine("Reservation updated successfully.");
                return true;
            }
            else
            {
                Console.WriteLine("Failed to update the guided tour.");
                return false;
            }
        }
        else
        {
            Console.WriteLine("Reservation not found.");
            return false;
        }
    }


    public void CancelReservation(string ticketCode)
    {
        string reservationsFilePath = "./JSON-Files/reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(reservationsFilePath)) ?? new List<dynamic>();

        var reservationToRemove = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservationToRemove != null)
        {
            // Accessing TourHour directly assuming it's always present and is an integer.
            int tourHour = (int)reservationToRemove.TourHour;

            reservations.Remove(reservationToRemove);
            File.WriteAllText(reservationsFilePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));

            if (guidedTour.RemoveVisitorFromTour(tourHour, ticketCode))
            {
                Console.WriteLine("Reservation cancelled successfully.");
                guidedTour.SaveGuidedToursToFile(); // Save the tours after updating
            }
            else
            {
                Console.WriteLine("Failed to remove the visitor from the guided tour.");
            }
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }


    public void SaveReservation(Visitor visitor)
    {
        // Define the path to the reservations file.
        string filePath = "./JSON-Files/reservations.json";

        // Read the existing reservations and deserialize them into a list of dynamic objects.
        List<dynamic> reservations = File.Exists(filePath)
            ? JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>()
            : new List<dynamic>();

        // Look for an existing reservation with the same TicketCode and TourHour.
        var existingReservation = reservations
            .FirstOrDefault(r => r.TicketCode == visitor.TicketCode && r.TourHour == visitor.RondleidingChoice);

        if (existingReservation != null)
        {
            // If found, add the new visitor's name to the existing reservation's Visitors list.
            existingReservation.Visitors.Add(visitor.Name);
        }
        else
        {
            // If not found, create a new reservation object with the TicketCode, TourHour, and the visitor's name.
            var newReservation = new
            {
                TicketCode = visitor.TicketCode,
                TourHour = visitor.RondleidingChoice,
                Visitors = new List<string> { visitor.Name }
            };

            // Add the new reservation object to the list of reservations.
            reservations.Add(newReservation);
        }

        // Serialize the list of reservations back to JSON.
        string updatedReservationsJson = JsonConvert.SerializeObject(reservations, Formatting.Indented);

        // Write the updated JSON to the reservations file.
        File.WriteAllText(filePath, updatedReservationsJson);
    }


    private bool VisitorAlreadyHasReservation(string ticketCode)
    {
        string filePath = "./JSON-Files/reservations.json";
        if (File.Exists(filePath))
        {
            var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>();
            return reservations.Any(r => r.TicketCode == ticketCode);
        }
        return false;
    }
}
