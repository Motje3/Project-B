using ReservationSystem;

public class Reservation : View
{
    public static void ValidateCodeAndShowMenu()

    {
        try { Console.Clear(); } catch { }

        // ASCI.Art();

        while (true)
        {
            ASCI.Art();
            string userCode = TicketInputRL.Show();

            if (userCode == "GETMEOUT")
            {
                return;
            }
            if (userCode == "5544332211")
            {
                AdminLoginMenu.ProcessLoginForm();
                continue;
            }
            else if (userCode == "1122334455")
            {
                try { Console.Clear(); } catch { }
                GuideLoginMenu.ProcessLoginForm();
                continue;
            }
            else if (Ticket.IsCodeValid(userCode))
            {
                Visitor currentVisitor = Visitor.FindVisitorByTicketCode(userCode);
                if (currentVisitor == null)
                {
                    currentVisitor = new Visitor(userCode);
                }

                WelcomeMessage.WelcomeYourTicketConfirmed();
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
            string choice = EditCurrTourMenuRL.Show(reservationDetails);
            choosingOption = logic.HandleFullMenuChoice(choice, visitor);

        }
    }

}
