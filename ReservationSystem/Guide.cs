public class Guide : Visitor
{
    new List<Visitor> Present = new List<Visitor>();
    public Guide(string name, string ticketcode) : base(name, ticketcode) {}
    public override string ToString()
    {
        return $"Name: {Name}\nTicket code: {TicketCode}";
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