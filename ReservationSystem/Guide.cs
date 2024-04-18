public class Guide : Visitor
{

    public Guide(string ticketcode) : base(ticketcode) {}
    public override string ToString()
    {
        return $"Ticket code: {TicketCode}";
    }
}

    public void CompleteTour() 
    {
        var newTour = GuidedTour.FindTourById(AssignedTourId)
        var oldTour = GuidedTour.FindTourById(**AssignedTourId**)
        if (AssignedTourId == null)
        {
            return;
        }
        if (newTour == null)
        {
            return;
        }
        if {newTour.Deleted == true}
        {
            return;
        }
        newTour.Completed = true;
        GuidedTour.EditTourInJSON(oldTour, newTour);
    }
    
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