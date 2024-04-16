public class Guide : Visitor
{
    public Guide( string ticketcode) : base( ticketcode) {}
    public override string ToString()
    {
        return $"Ticket code: {TicketCode}";
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