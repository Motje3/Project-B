using ReservationSystem;

public class ReservationPresentation : View
{
    public static void ValidateCodeAndShowMenu()

    {
        try { Console.Clear(); } catch { }


        Console.WriteLine("Welcome to the Museum!\n");


        while (true)
        {
            string userCode = TicketInputRL.Show();

            if (userCode == "GETMEOUT")
            {
                return;
            }
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

                WelcomeMessage.Show();
                if (currentVisitor.HasReservation(currentVisitor))
                {
                    ShowFullMenu(currentVisitor);
                }
                else
                {
                    ShowRestrictedMenu(currentVisitor);
                }
                continue;
            }
            else
            {
                TicketCodeFailed.Show();
            }
        }
    }


    // The fellowing 2 methods are from MenuLogic:
    // "Bora" moved it here because he thought it was more suited here.
    // And yes i speak from 3th-Person

    private static void ShowRestrictedMenu(Visitor visitor)
    {
        MenuLogic.JoinTour(visitor);
    }

    private static void ShowFullMenu(Visitor visitor)
    {
        MenuLogic logic = new MenuLogic();
        bool choosingOption = true;
        while (choosingOption)
        {
            string reservationDetails = Visitor.GetCurrentReservation(visitor);
            // Console.WriteLine(reservationDetails);
            // Console.WriteLine("\nPlease choose an option:");
            // Console.WriteLine("1. Change my reservation time");
            // Console.WriteLine("2. Cancel my tour reservation");
            // Console.WriteLine("3. Return to main menu");
            // Console.Write("\nEnter your choice: ");
            // string choice = Console.ReadLine();
            string choice = EditCurrTourMenuRL.Show(reservationDetails);

            choosingOption = logic.HandleFullMenuChoice(choice, visitor);

        }
    }

    public static void PrintAllGuides()
    {
        Console.WriteLine("All Guides:");
        foreach (var guide in Guide.AllGuides)
        {
            Console.WriteLine($"Name: {guide.Name}, GuideId: {guide.GuideId}");
        }
    }


}
