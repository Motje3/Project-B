using Newtonsoft.Json;
public class ReservationManager
{
    private EntreeCodeValidator validator;
    private GuidedTour guidedTour;
    private MenuManager menumanager;

    public ReservationManager()
    {
        validator = new EntreeCodeValidator();
        guidedTour = new GuidedTour();
        menumanager = new MenuManager(this, guidedTour); // Pass the same GuidedTour instance
        guidedTour.LoadToursFromFile("./JSON-Files/guidedTours.json");
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
                    menumanager.ShowRestrictedMenu(userCode);
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
        var reservation = new
        {
            visitor.TicketCode,
            visitor.VisitorId,
            visitor.Name,
            TourHour = visitor.RondleidingChoice
        };

        List<object> reservations = new List<object>();
        string filePath = "./JSON-Files/reservations.json";
        if (File.Exists(filePath))
        {
            string existingReservationsJson = File.ReadAllText(filePath);
            reservations = JsonConvert.DeserializeObject<List<object>>(existingReservationsJson) ?? new List<object>();
        }

        reservations.Add(reservation);
        string updatedReservationsJson = JsonConvert.SerializeObject(reservations, Formatting.Indented);
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
