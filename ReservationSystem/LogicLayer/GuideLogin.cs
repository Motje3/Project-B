using ReservationSystem;
using System;

public class GuideLoginMenu : View
{
    public static void ProcessLoginForm(string userCode)
    {
        Guide.LoadGuides(); // Ensure guides are loaded

        GuideLogin.Show();
        string password = AdminBackEnd.ReadPassword();

        Guide authenticatedGuide = Guide.AuthenticateGuide(password);

        if (authenticatedGuide != null)
        {
            AccessPassed.WelcomeGuide(authenticatedGuide);
            ShowGuideMenu(authenticatedGuide);
        }
        else
        {
            AccessFailed.Show();
        }
    }

    public static void ShowGuideMenu(Guide guide)
    {
        bool continueRunning = true;
        while (continueRunning)
        {
            string choice = GuideMenuRL.Show();

            switch (choice)
            {
                case "1":
                    Guide.ViewPersonalTours(guide);
                    break;
                case "2":
                    
                case "3":
                    AddVisitorLastMinuteValidation(guide);
                    break;
                case "4":
                    continueRunning = false;
                    try { Console.Clear(); } catch { }
                    break;
                default:
                    InvalidRL.Show();
                    break;
            }
        }
    }

    // failed trasnfer messages is provided in guide.AddVisitorLastMinute(visitor)
    private static void AddVisitorLastMinuteValidation(Guide guide)
    {
        string ticketCode = ScanVisitor.Show();
        Ticket.LoadTickets();
        if (Ticket.Tickets.Contains(ticketCode))
        {
            Visitor visitor = new Visitor(ticketCode);
            if (HasReservation(visitor))  // check if visitor already has reservation
            {
                TransferChoiceRL.ShowMessage();
                while (true)
                {
                    string choice = TransferChoiceRL.ShowChoice();
                    if (choice == "Y" || choice == "Yes" || choice == "1")
                    {
                        Tour tour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);
                        Visitor oldVisitor = tour.ExpectedVisitors.Where(v => v.TicketCode == ticketCode).ToList()[0];  // to grap old Visitor-Data for removal.
                        tour.RemoveVisitor(oldVisitor);  // Visitor removed from Expected Visitor
                        Tour tourDetail = guide.AddVisitorLastMinute(oldVisitor);  // tranfer visitor to next tour
                        if (tourDetail != null)
                        {
                            TransferSucces.Show(tourDetail);
                            return;
                        }
                        return;
                    }
                    if (choice == "N" || choice == "No" || choice == "2")
                    {
                        TransferCanceled.Show();
                        return; 
                    }
                    else
                    {
                        InvalidRL.Show();
                        // continue while loop until valid choice
                    }
                }
            }
            else  // Add visitor automaticly if it has no reservation
            {
                Tour tourDetail = guide.AddVisitorLastMinute(visitor);  // futher message provided in AddVisitorLastMinute method if null.
                if (tourDetail != null)
                {
                    TransferSucces.Show(tourDetail);
                    return;
                }
                return;
            }
        }
        InvalidTicketCode.Show(ticketCode);
        // Console.WriteLine($"The code ['{ticketCode}'] is invalid");
        // Console.WriteLine("please provide a valid visitor ticketCode");
    }

    private static bool HasReservation(Visitor visitor)
    {
        return Tour.TodaysTours.Any(tour => tour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode));  // checks TicketCode without the VisitorID object
    }
}

        
