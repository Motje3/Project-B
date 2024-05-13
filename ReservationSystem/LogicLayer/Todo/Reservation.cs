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

                Console.WriteLine("\nWelcome! Your ticket is confirmed\n");
                if (currentVisitor.HasReservation(currentVisitor))
                {
                    MenuLogic.ShowFullMenu(currentVisitor);
                }
                else
                {
                    MenuLogic.ShowRestrictedMenu(currentVisitor);
                }
                continue;
            }
            else
            {
                WriteLine("Sorry, your ticket is not valid. Please try again.");
            }
        }
    }

    public static void ShowRestrictedMenu(Visitor visitor)
    {
        MenuLogic.JoinTour(visitor);
    }

    public static void ShowFullMenu(Visitor visitor)
    {
        MenuLogic logic = new MenuLogic();
        bool choosingOption = true;
        while (choosingOption)
        {
            string reservationDetails = Visitor.GetCurrentReservation(visitor);
            Console.WriteLine(reservationDetails);
            Console.WriteLine("\nPlease choose an option:");
            Console.WriteLine("1. Change my reservation time");
            Console.WriteLine("2. Cancel my tour reservation");
            Console.WriteLine("3. Return to main menu");
            Console.Write("\nEnter your choice: ");
            string choice = Console.ReadLine();

            choosingOption = logic.HandleFullMenuChoice(choice, visitor);

        }
    }

}
