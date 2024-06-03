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

            var ticket = upcomingTour.ExpectedVisitors.Select(v => v.TicketCode).Where(t => t == input).FirstOrDefault();

            if (ticket != null)
            {
                if (!upcomingTour.PresentVisitors.Any(v => v.TicketCode == ticket))
                {
                    Visitor visitorWithTicket = upcomingTour.ExpectedVisitors.Where(v => v.TicketCode == ticket).FirstOrDefault();
                    upcomingTour.PresentVisitors.Add(visitorWithTicket);
                    SoundsPlayer.PlaySound(SoundsPlayer.SoundFile.ChceckIn);
                    TourDataManager.SaveTours();
                }
                else
                    GuideHasAlreadyCheckedInVisitor.Show(ticket);
            }
            else if (input != "start")
                GuideScannedInvalidTicket.Show();
        }

        try { Console.Clear(); } catch { }
        AddVisitorLastMinuteValidation(guide, upcomingTour);

        upcomingTour.Started = true;
        TourDataManager.SaveTours();

        try { Console.Clear(); } catch { }
        GuideHasSuccesfullyStartedTour.Show(upcomingTour);
        return true;
    }

    // failed trasnfer messages is provided in guide.AddVisitorLastMinute(visitor)
    private static void AddVisitorLastMinuteValidation(Guide guide, Tour upcomingTour)
    {
        Ticket.LoadTickets();

        string ticketCode = "";
        while (ticketCode != "start")
        {
            GuideAddingVisitorLastMinute.Show();
            ticketCode = Program.World.ReadLine().ToLower();

            if (ticketCode == "start")
                return;

            if (!Ticket.Tickets.Contains(ticketCode))
            { 
                InvalidTicketCode.Show(ticketCode);
                continue; 
            }
            
            if (upcomingTour.PresentVisitors.Select(v => v.TicketCode).Any(t => t == ticketCode))
            {
                try { Console.Clear(); }catch{}
                GuideHasAlreadyCheckedInVisitor.Show(ticketCode);
                continue;
            }

            Visitor visitor = new Visitor(ticketCode);

            if (HasReservation(visitor))  // check if visitor already has reservation
            {
                string choice = "";
                while (true)
                {
                    TransferChoiceRL.ShowMessage();
                    TransferChoiceRL.ShowChoice();
                    
                    choice = Program.World.ReadLine();
                    if (choice == "Y" || choice == "Yes" || choice == "1")
                    {  
                        Tour tour = TourTools.FindTourByVisitorTicketCode(visitor.TicketCode);
                        Visitor oldVisitor = tour.ExpectedVisitors.Where(v => v.TicketCode == ticketCode).ToList()[0];  // to grap old Visitor-Data for removal.
                        tour.RemoveVisitor(oldVisitor);  // Visitor removed from Expected Visitor
                        upcomingTour.PresentVisitors.Add(oldVisitor);  // tranfer visitor to next tour
                        upcomingTour.ExpectedVisitors.Add(oldVisitor);
                        TourDataManager.SaveTours();
                        
                        try { Console.Clear(); } catch { }
                        TransferSucces.Show(oldVisitor, upcomingTour);
                        break;
                    }
                    if (choice == "N" || choice == "No" || choice == "2")
                    {
                        try { Console.Clear(); } catch { }
                        TransferCanceled.Show();
                        break;
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
                upcomingTour.PresentVisitors.Add(visitor);  // futher message provided in AddVisitorLastMinute method if null.
                TourDataManager.SaveTours();

                try { Console.Clear(); } catch { }
                TransferSucces.Show(visitor, upcomingTour);
            }
        }
    }

    private static bool HasReservation(Visitor visitor)
    {
        return TourTools.TodaysTours.Any(tour => tour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode));  // checks TicketCode without the VisitorID object
    }
}


