public class Visitor
{
    public Guid VisitorId { get; private set; }
    public string TicketCode { get; private set; } // Make TicketCode immutable from outside

    // Constructor with ticket code parameter to ensure all visitors have a ticket code when created
    public Visitor(string ticketCode)
    {
        VisitorId = Guid.NewGuid();
        TicketCode = ticketCode; // Initialize ticket code upon creation
    }

    // Checks if the visitor has a reservation in any of the current tours
    public bool HasReservation(Visitor visitor)
    {
        return Tour.TodaysTours.Any(tour => tour.ExpectedVisitors.Contains(this));
    }

    // Static method to find a visitor by ticket code in the list of current tours
    public static Visitor FindVisitorByTicketCode(string ticketCode)
    {
        return Tour.TodaysTours.SelectMany(tour => tour.ExpectedVisitors).FirstOrDefault(visitor => visitor?.TicketCode == ticketCode);
    }

    public static string GetCurrentReservation(Visitor visitor)
    {
        // Find the tour that this visitor is part of by searching for the ticket code
        var assignedTour = Tour.TodaysTours.FirstOrDefault(tour => tour.ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode));

        if (assignedTour != null)
        {
            // If the tour is found, return formatted reservation details
            string formattedStartTime = assignedTour.StartTime.ToString("HH:mm");


            // return $"\nYour current reservation is at {formattedStartTime}.\n";
            string reservationMessage = "\nYour current reservation is at ";
            string coloredFormattedStartTime = ColourText.GetColoredString("", formattedStartTime, ConsoleColor.Cyan, "");
            return reservationMessage + coloredFormattedStartTime + "\n";
        }
        else
        {
            // If no reservation is found, return this message
            return "\nYou currently have no reservation.\n";
        }
    }


}
