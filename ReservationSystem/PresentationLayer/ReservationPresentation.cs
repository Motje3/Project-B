using ReservationSystem;

public class ReservationPresentation : View 
{
    public static void ValidateCodeAndShowMenu()
    {
        try { Console.Clear(); } catch { }

        while (true)
        {
            WriteLine("Enter your unique ticket code:");
            string userCode = ReadLine();

            if (userCode == "123")
            {
                AdminLoginMenu.ProcessLoginForm();
                continue;
            }
            else if (userCode == "456")
            {
                GuideLoginMenu.ProcessLoginForm(userCode);
                continue;
            }
            else if (Ticket.IsCodeValid(userCode))
            {
                Visitor currentVisitor = Visitor.FindVisitorByTicketCode(userCode);
                if (currentVisitor == null)
                {
                    currentVisitor = new Visitor(userCode);
                }

                WriteLine("\nWelcome, your ticket is confirmed!\n");
                if (currentVisitor.HasReservation(currentVisitor))
                {
                    MenuPresentation.ShowFullMenu(currentVisitor);
                }
                else
                {
                    MenuPresentation.ShowRestrictedMenu(currentVisitor);
                }
                continue; // Continue will start the loop from the beginning
            }
            else
            {
                WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }

}
