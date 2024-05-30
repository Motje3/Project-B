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
        try { Console.Clear(); } catch { }
        bool continueRunning = true;
        while (continueRunning)
        {

            AccessPassed.WelcomeGuide(guide);
            string choice = GuideMenuRL.Show();

            switch (choice)
            {
                case "1":
                    try { Console.Clear(); } catch { }
                    guide.ViewPersonalTours();
                    break;
                case "2":
                    try { Console.Clear(); } catch { }
                    StartUpcomingTour(guide);
                    break;
                case "3":
                    try { Console.Clear(); } catch { }
                    AddVisitorLastMinuteValidation(guide);
                    break;
                case "4":
                    continueRunning = false;
                    try { Console.Clear(); } catch { }
                    break;

                default:
                    {
                        InvalidRL.Show();
                        break;
                    }
            }
        }
    }

    public static bool  StartUpcomingTour(Guide guide)
    {
        Tour? upcomingTour = TourTools.TodaysTours
            .Where(t => !t.Started && !t.Deleted && t.AssignedGuide.Name == guide.Name && t.StartTime > Program.World.Now)
            .OrderBy(t => t.StartTime)
            .FirstOrDefault();

        if (upcomingTour == null)
        {
            Program.World.WriteLine("No upcoming tours available to start.");
            return false;
        }

        string input = "";
        while (input != "start")
        {
            GuideStartingTourMessage.Show(upcomingTour);
            input = Program.World.ReadLine().ToLower();
            
            // Early return for going back
            if (input == "q")
            { 
                try { Console.Clear(); }catch{}
                return false;
            }
            
            try { Console.Clear(); }catch{}

            var ticket = Visitor.FindVisitorByTicketCode(input);

            if (ticket != null)
            {
                if (!upcomingTour.PresentVisitors.Any(v => v.TicketCode == ticket.TicketCode))
                {
                    upcomingTour.PresentVisitors.Add(ticket);
                    SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.ChceckIn);
                    TourDataManager.SaveTours();
                }
                else
                    GuideHasAlreadyCheckedInVisitor.Show();
            }
            else if (input != "start")
                GuideScannedInvalidTicket.Show();
        }

        upcomingTour.Started = true;
        TourDataManager.SaveTours();
        try { Console.Clear(); } catch { }
        Program.World.WriteLine("Tour has been started successfully.");
        return true;
    }

    // failed trasnfer messages is provided in guide.AddVisitorLastMinute(visitor)
    private static void AddVisitorLastMinuteValidation(Guide guide)
    {
        List<Tour> tours = TourTools.TodaysTours.Where(t => t.AssignedGuide.Name == guide.Name && t.Started == false && t.StartTime > Program.World.Now).ToList();
        if (tours.Count == 0)
        {
            Program.World.WriteLine("You have no more tours today!");
            return;
        }
        string ticketCode = ScanVisitor.Show();

        if (ticketCode == "Q")
            return;
        Ticket.LoadTickets();
        while (!Ticket.Tickets.Contains(ticketCode))
        {
            if (ticketCode == "Q")
                return;
            InvalidTicketCode.Show(ticketCode);
            // Console.WriteLine($"The code ['{ticketCode}'] is invalid");
            // Console.WriteLine("please provide a valid visitor ticketCode");
            ticketCode = Program.World.ReadLine();
        }

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
                        Tour tour = TourTools.FindTourByVisitorTicketCode(visitor.TicketCode);
                        Visitor oldVisitor = tour.ExpectedVisitors.Where(v => v.TicketCode == ticketCode).ToList()[0];  // to grap old Visitor-Data for removal.
                        tour.RemoveVisitor(oldVisitor);  // Visitor removed from Expected Visitor
                        Tour tourDetail = guide.AddVisitorLastMinute(oldVisitor);  // tranfer visitor to next tour
                        if (tourDetail != null)
                        {
                            try { Console.Clear(); } catch { }
                            TransferSucces.Show(tourDetail);
                            return;
                        }
                        return;
                    }
                    if (choice == "N" || choice == "No" || choice == "2")
                    {
                        try { Console.Clear(); } catch { }
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
    }

    private static bool HasReservation(Visitor visitor)
    {
        return TourTools.TodaysTours.Any(tour => tour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode));  // checks TicketCode without the VisitorID object
    }
}


