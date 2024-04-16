// **CheckInVisitor **moet variable **GuidedTour tour = GuidedTour.FindTourById(AssingedTourId)**

public class Guide : Visitor
{
    public Guide(string name, string ticketcode) : base(name, ticketcode) {}
    public override string ToString()
    {
        return $"Name: {Name}\nTicket code: {TicketCode}";
    }
    public void CheckInVisitor(Visitor visitor)
    {
        GuidedTour tour = GuidedTour.FindTourById(base.AssingedTourId);

        if (tour.ExpectedVisitors.Contains(visitor))
        {
            Console.WriteLine($"\nWelcome {visitor.Name} to our museum");
            return;
        }
        if (tour.PresentVisitors.Contains(visitor))
        {
            Console.WriteLine($"\nVisitor {visitor.Name} already joine the tour");
            return;
        }
        if (!tour.PresentVisitors.Contains(visitor))
        {
            tour.PresentVisitors.Add(visitor);
            Console.WriteLine($"\nVisitor is not Registerd");
            return;
        }  
    }
}

    // public void CompleteTour() 
    // {

    // }
    // public void TourInProgress()
    // {

    // }
    // public void PresentVisitors()
    // {
    //     foreach (Visitor visitor in Present)
    //     {
    //         Console.WriteLine(visitor);
    //     }
    // }