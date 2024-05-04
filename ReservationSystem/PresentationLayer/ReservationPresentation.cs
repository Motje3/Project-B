public static class ReservationPresentation
{
    public static void ValidateCodeAndShowMenu()
    {
        // Clear console if not debugging
        try { Console.Clear(); } catch { }

        bool LoopOption = true;
        while (LoopOption)
        {
            Console.WriteLine("Enter your unique ticket code:");
            string userCode = Console.ReadLine();


            if (userCode == "123")
            {
                AdminLoginMenu.ProcessLoginForm();
                ValidateCodeAndShowMenu();
            }
            else if (userCode == "456")
            {
                GidsLoginProcessor.ProcessLoginForm(userCode);
                ValidateCodeAndShowMenu();
            }
            else if (Ticket.IsCodeValid(userCode))
            {
                Visitor currentVisitor = Visitor.FindVisitorByTicketCode(userCode);

                if (currentVisitor != null)
                {
                    Console.WriteLine("\nWelcome, your ticket is confirmed!\n");
                    LoopOption = false;

                    if (currentVisitor.HasReservation(currentVisitor))
                    {
                        MenuPresentation.ShowFullMenu(currentVisitor);
                    }
                }
                else
                {
                    currentVisitor = new Visitor (userCode);
                    MenuPresentation.ShowRestrictedMenu(currentVisitor);
                    LoopOption = false;
                }
                ValidateCodeAndShowMenu();
            }
            else
            {
                Console.WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }
}
