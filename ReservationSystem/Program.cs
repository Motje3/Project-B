﻿public class Program
{
    public static void Main()

    {
        //ReservationManager.ValidateCodeAndProcessReservations();

        //DateTime testTime = new(2024, 4, 10, 14, 20, 0);
        //Guid testID = Guid.NewGuid();
        //GuidedTour test = new(testTime, GuidedTour.CompletedTours[0].TourId);
        //GuidedTour test2 = new(GuidedTour.CurrentTours[0].StartTime.AddMinutes(40),test.TourId);
        //GuidedTour.EditTourInJson(test,test2);
        //GuidedTour.AddTourToJSON(test);
        //GuidedTour.RemoveTourFromJSON(test);
        //var test6 = GuidedTour.ReturnAllToursFromThisYear();
        //var test3 = GuidedTour.ReturnAllCurrentToursFromToday();
        //var completed = GuidedTour.CompletedTours;
        //var current = GuidedTour.CurrentTours;
        //var deleted = GuidedTour.DeletedTours;
        //var toursOfToDay = GuidedTour.ReturnAllCurrentToursFromToday();
        Visitor testVisitor = new Visitor("Alice", "111");
        var chosenTour = GuidedTour.ReturnAllCurrentToursFromToday()[0];
        var newChosenTour = chosenTour.Clone();
        newChosenTour.ExpectedVisitors.Add(testVisitor);
        GuidedTour.EditTourInJSON(chosenTour, newChosenTour);
    }

    // This will take a couple of minutes do not use unless REALLY nesecasry
    public static void AddAllToursToJson()
    {
        foreach (var tour in GuidedTour.ReturnAllToursFromThisYear())
        {
            GuidedTour.AddTourToJSON(tour);
        }
    }
}
