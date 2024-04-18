public static class ReservationManager
{
    public static void ValidateCodeAndProcessReservations()
    {
        // Clear console if not debugging
        try{Console.Clear();}catch{} 
        
        bool isValidCode = false;
        while (!isValidCode)
        {
            Console.WriteLine("Enter your unique ticket code:");
            string userCode = Console.ReadLine();


            if (userCode == "123")
            {
                AdminLoginProcessor.ProcessLoginForm();
                
                // Repeat this method until app closed
                ValidateCodeAndProcessReservations();
            }
            else if (userCode == "456")
            {
                GidsLoginProcessor.ProcessLoginForm(userCode);
                
                // Repeat this method until app closed
                ValidateCodeAndProcessReservations();
            }
            else if (Ticket.IsCodeValid(userCode))
            {
                Visitor currentVisitor = Visitor.FindVisitorByTicketCode(userCode);
                
                if (currentVisitor != null)
                {
                    Console.WriteLine("Welcome, your ticket is confirmed!\n");
                    isValidCode = true;

                    if (currentVisitor.HasReservation())
                    {
                        MenuManager.ShowFullMenu(currentVisitor);
                    }
                }
                //Else visitor was not found == methode returned null...
                else
                {
                    MenuManager.ShowRestrictedMenu(userCode); // Ensure your ShowRestrictedMenu can handle null
                    isValidCode = true; // Consider whether you want to set this to true if no visitor is found
                }

                // Repeat this method until app closed
                ValidateCodeAndProcessReservations();
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }
}
