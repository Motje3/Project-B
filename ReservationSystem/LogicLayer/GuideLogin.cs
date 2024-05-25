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
                    AddVisitorLastMinuteValidation(guide);
                    break;
                case "3":
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
            Visitor visitor = Visitor.FindVisitorByTicketCode(ticketCode);
            if (visitor != null)
            {
                if (visitor.HasReservation(visitor))  // check if visitor already has reservation
                {
                    TransferChoiceRL.ShowMessage();
                    while (true)
                    {
                        string choice = TransferChoiceRL.ShowChoice();
                        if (choice == "Y" || choice == "Yes" || choice == "1")
                        {
                            Tour tour = Tour.FindTourByVisitorTicketCode(visitor.TicketCode);
                            tour.RemoveVisitor(visitor);  // Visitor removed from Expected Visitor
                            Tour tourDetail = guide.AddVisitorLastMinute(visitor);
                            TransferSucces.Show(tourDetail);  // Show succes-messsage and detail of wich tour (time) visitor is added.
                            break;  // breaking out of while loop 
                        }
                        if (choice == "N" || choice == "No" || choice == "2")
                        {
                            TransferCanceled.Show();
                            break;  // breaking out of while loop 
                        }
                        else
                        {
                            InvalidRL.Show();
                            // continue while loop until valid choice
                        }
                    }
                }
                else
                {
                    Tour tourDetail = guide.AddVisitorLastMinute(visitor);  // futher message provided in method if null
                    if (tourDetail != null)
                    {
                        TransferSucces.Show(tourDetail);
                    }
                    
                }
            }
        }
        InvalidTicketCode.Show(ticketCode);
        // Console.WriteLine($"The code ['{ticketCode}'] is invalid");
        // Console.WriteLine("please provide a valid visitor ticketCode");
    }
}
