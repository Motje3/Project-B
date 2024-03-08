using System;

public class Program
{
    public static void Main()
    {
        
        //string jsonFilePath = @"C:\Users\moham\Desktop\School\Project-B\ReservationSystem\sample_codes.json";
        var reservationManager = new ReservationManager();

        reservationManager.ValidateCodeAndProcessReservations();

    }
}
