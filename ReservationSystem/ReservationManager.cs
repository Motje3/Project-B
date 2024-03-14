using Newtonsoft.Json;
using System.IO;
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

            if (userCode == "123")
            {
                AdminLoginProcessor adminManager = new AdminLoginProcessor();
                adminManager.ProcessLoginForm();
                return; // Exit the method after admin login form is processed
            }
            else if (validator.IsCodeValid(userCode))
            {
                Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                isValidCode = true; // Update the flag since the code is valid

                bool choosingOption = true;
                while (choosingOption)
                {
                    Console.WriteLine("\nPlease choose an option:");
                    Console.WriteLine("1. Join a tour");
                    Console.WriteLine("2. Edit my reservation");
                    Console.WriteLine("3. Cancel my reservation");
                    Console.WriteLine("4. Exit");
                    Console.Write("\nEnter your choice: ");

                    string choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "1":

                            guidedTour.ListAvailableTours();
                            Console.WriteLine("Please enter the hour of the tour you wish to join (9 to 17):");
                            int chosenHour;

                            if (int.TryParse(Console.ReadLine(), out chosenHour))
                            {
                                // Here we create the Visitor instance with the chosen hour and the ticket code
                                Visitor visitor = new Visitor("Name", chosenHour, userCode);

                                if (VisitorAlreadyHasReservation(visitor.TicketCode))
                                {
                                    Console.WriteLine("You have already joined a tour. To join a different tour, please first cancel your existing reservation.");
                                    break; 
                                } 
                                
                                else if (guidedTour.JoinTour(chosenHour, visitor))
                                {
                                    Console.WriteLine($"You've successfully joined the {chosenHour}:00 tour.");
                                    SaveReservation(visitor);
                                    break;
                                }
                                else
                                {
                                    Console.WriteLine("Failed to join the tour. It might be full or the selection was invalid.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid tour hour.");
                            }
                            break;

                        case "2":
                            Console.Write("Enter your ticket code: ");
                            string ticketCodeForEdit = Console.ReadLine();
                            Console.Write("Enter the new tour hour (9 to 17): ");
                            int newTourHour;
                            if (int.TryParse(Console.ReadLine(), out newTourHour))
                            {
                                EditReservation(ticketCodeForEdit, newTourHour);
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid hour.");
                            }
                            break;

                        case "3":
                            Console.Write("Enter your ticket code to cancel: ");
                            string ticketCodeForCancel = Console.ReadLine();
                            CancelReservation(ticketCodeForCancel);
                            break;

                        case "4":
                            choosingOption = false;
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }


    public void EditReservation(string ticketCode, int newTourHour)
    {
        string filePath = "reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>();

        var reservation = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservation != null)
        {
            reservation.RondleidingChoice = newTourHour;
            reservation.TourHour = newTourHour; // Assuming RondleidingChoice represents the hour.

            File.WriteAllText(filePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));
            Console.WriteLine("Reservation updated successfully.");
        }
        else
        {
            Console.WriteLine("Reservation not found.");
        }
    }


    public void CancelReservation(string ticketCode)
    {
        string filePath = "reservations.json";
        var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>();

        var reservationToRemove = reservations.FirstOrDefault(r => r.TicketCode == ticketCode);
        if (reservationToRemove != null)
        {
            reservations.Remove(reservationToRemove);
            File.WriteAllText(filePath, JsonConvert.SerializeObject(reservations, Formatting.Indented));
            Console.WriteLine("Reservation cancelled successfully.");
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
            visitor.RondleidingChoice,
            TourHour = visitor.RondleidingChoice // Assuming RondleidingChoice is the hour
        };

        List<object> reservations = new List<object>();
        string filePath = "reservations.json";
        if (File.Exists(filePath))
        {
            // Load existing reservations
            string existingReservationsJson = File.ReadAllText(filePath);
            reservations = JsonConvert.DeserializeObject<List<object>>(existingReservationsJson) ?? new List<object>();
        }

        reservations.Add(reservation);
        string updatedReservationsJson = JsonConvert.SerializeObject(reservations, Formatting.Indented);
        File.WriteAllText(filePath, updatedReservationsJson);
    }

    private bool VisitorAlreadyHasReservation(string ticketCode)
    {
        string filePath = "reservations.json";
        if (File.Exists(filePath))
        {
            var reservations = JsonConvert.DeserializeObject<List<dynamic>>(File.ReadAllText(filePath)) ?? new List<dynamic>();
            return reservations.Any(r => r.TicketCode == ticketCode);
        }
        return false;
    }

}
