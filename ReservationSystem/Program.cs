public class Program
{
    public static void Main()

    {
        //var reservationManager = new ReservationManager();
        //reservationManager.ValidateCodeAndProcessReservations();
        DateTime testTime = new(2024, 10, 10, 14, 40, 0);
        GuidedTour test = new(testTime);
        GuidedTour.AddTourToJSON(test);
    }
}
