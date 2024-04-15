public static class ReservationManager
{
    public static void ValidateCodeAndProcessReservations()
    {
        bool isValidCode = false;
        while (!isValidCode)
        {
            Console.WriteLine("Enter your unique ticket code:");
            string userCode = Console.ReadLine();


            if (userCode == "123")
            {
                AdminLoginProcessor.ProcessLoginForm();
                return; // Exit the method after admin login form is processed
            }
            else if (userCode == "456")
            {
                GidsLoginProcessor.ProcessLoginForm();
                return; //Exit the method after guide login form is processed
            }
            else if (EntreeCodeValidator.IsCodeValid(userCode))
            {
                //ASK PO ABOUT THIS WHETHER ONLINETICKETS WILL HAVE NAMES. 
                //ASK PO ABOUT THIS WHETHER ONLINETICKETS WILL HAVE NAMES.
                //ASK PO ABOUT THIS WHETHER ONLINETICKETS WILL HAVE NAMES.
                //ASK PO ABOUT THIS WHETHER ONLINETICKETS WILL HAVE NAMES.
                Visitor currentVisitor = Visitor.FindVisitorByTicketCode(userCode);
                if (currentVisitor != null)
                {
                    Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                    isValidCode = true;

                    bool visitorHasReservation = currentVisitor.HasReservation();
                    if (visitorHasReservation)
                    {
                        MenuManager.ShowFullMenu(currentVisitor);
                    }
                }
                //Else visitor was not found == methode returned null...
                else
                {
                    MenuManager.ShowRestrictedMenu(null); // Ensure your ShowRestrictedMenu can handle null
                    isValidCode = true; // Consider whether you want to set this to true if no visitor is found
                }
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }


    // To be removed
    /*public bool EditReservation(string ticketCode, DateTime newTourDateTime)
    {
        string reservationsFilePath = "./JSON-Files/reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(reservationsFilePath)) ?? new List<dynamic>();

        var reservation = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservation != null)
        {
            // Parse the old tour time as DateTime
            DateTime oldTourDateTime = DateTime.Parse(reservation.TourHour.ToString());

            // Attempt to update the tour time for the visitor.
            bool updateSuccess = guidedTour.UpdateVisitorTour(ticketCode, newTourDateTime);
            if (updateSuccess)
            {
                // Update the reservation details with the new tour time.
                reservation.TourHour = newTourDateTime.ToString("o"); // Using the round-trip format specifier

                // Write the changes back to the reservations file.
                File.WriteAllText(reservationsFilePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));

                // Now, also update the guided tours file to reflect the change.
                guidedTour.SaveGuidedToursToFile(); // Assuming this method saves the entire state of `TourSlots` to a file.

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
    }*/

    // To be removed
    /*public void CancelReservation(string ticketCode)
    {
        string reservationsFilePath = "./JSON-Files/reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(reservationsFilePath)) ?? new List<dynamic>();

        var reservationToRemove = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservationToRemove != null)
        {
            // Assuming reservationToRemove.TourHour is a date-time string in the JSON file.
            DateTime tourDateTime;
            if (DateTime.TryParse(reservationToRemove.TourHour.ToString(), out tourDateTime))
            {
                reservations.Remove(reservationToRemove);
                File.WriteAllText(reservationsFilePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));

                if (guidedTour.RemoveVisitorFromTour(tourDateTime, ticketCode))
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
                Console.WriteLine("Failed to parse the tour hour from the reservation.");
            }
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }*/


    // To be removed
    /*public void SaveReservation(Visitor visitor)
    {
        // Define the path to the reservations file.
        string filePath = "./JSON-Files/reservations.json";

        // Read the existing reservations and deserialize them into a list of dynamic objects.
        List<dynamic> reservations = File.Exists(filePath)
            ? JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>()
            : new List<dynamic>();

        // Look for an existing reservation with the same TicketCode and TourHour.
        var existingReservation = reservations
            .FirstOrDefault(r => r.TicketCode == visitor.TicketCode && r.TourHour == visitor.TourTime);

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
                TourHour = visitor.TourTime,
                Visitors = new List<string> { visitor.Name }
            };

            // Add the new reservation object to the list of reservations.
            reservations.Add(newReservation);
        }

        // Serialize the list of reservations back to JSON.
        string updatedReservationsJson = JsonConvert.SerializeObject(reservations, Formatting.Indented);

        // Write the updated JSON to the reservations file.
        File.WriteAllText(filePath, updatedReservationsJson);
    }*/
}
