public class Program
{
    public static void Main()

    {
        //ReservationManager.ValidateCodeAndProcessReservations();
        DateTime testTime = new(2024, 4, 10, 14, 20, 0);
        //Guid testID = Guid.NewGuid();
        GuidedTour test = new(testTime, GuidedTour.CompletedTours[0].TourId);
        //GuidedTour test2 = new(GuidedTour.CurrentTours[0].StartTime.AddMinutes(40),test.TourId);
        //GuidedTour.EditTourInJson(test,test2);
        GuidedTour.AddTourToJSON(test);
        //GuidedTour.RemoveTourFromJSON(test);

        //var test3 = GuidedTour.ReturnAllCurrentToursFromToday();
    }
}
