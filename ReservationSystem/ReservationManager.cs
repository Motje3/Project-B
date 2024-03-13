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
                return; // Exit the method after login form is processed
            }

            else if (validator.IsCodeValid(userCode))
            {
                Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                isValidCode = true;

                string response = "";
                do
                {
                    Console.WriteLine("Please choose an option:\n\n1. Join a tour \n2. Edit an existing tour time \n3. Cancel your tour");
                    response = Console.ReadLine()?.Trim(); // Safely trim the response

                    switch (response)
                    {
                        case "1":
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
        //The logic to change your rondleiding time.
    }


    private void CancelReservation()
    {
        //Logic to cancel a rondleiding attached to visitor.
    }

}
