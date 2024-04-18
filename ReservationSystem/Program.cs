public class Program
{
    public static void Main()

    {
        //AddGuide();
        ReservationManager.ValidateCodeAndProcessReservations();
        
        //GidsLoginProcessor._askVisitorCode();
    }

    public static void AddGuide()
    {
        GuidedTour tour = GuidedTour.ReturnAllCurrentToursFromTommorow()[0];
        Guide John = new Guide("John Doe", "456", tour.TourId);
        Visitor Lili = new("Lilith", "666");
        
        tour.AddVisitor(John);
        tour.AddVisitor(Lili);
    }
}
