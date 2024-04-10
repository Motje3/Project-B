public class Program
{
    public static void Main()

    {
        //var reservationManager = new ReservationManager();
        //reservationManager.ValidateCodeAndProcessReservations();
        DateTime testTime = new(2024, 10, 10, 14, 20, 0);
        GuidedTour test = new(testTime,100000000);
        GuidedTour test2 = new(GuidedTour.CurrentTours[0].StartTime.AddMinutes(40),test.TourId);
        GuidedTour.EditTourInJson(test,test2);
        //GuidedTour.AddTourToJSON(test);
    }
}
