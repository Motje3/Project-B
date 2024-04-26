namespace ReservationSystem;

// This class has multiple Show methods to prevent overflow of classes in View folder
// Make sure to assing method to the right condition
public class GidsDisplayTourInfo : View
{
    // 0 visitor expected in next tour
    public void ShowEmpty(DateOnly dateOnly, TimeOnly startTimeOnly, TimeOnly endTimeOnly, int amount = 0)
    {
        WriteLine($"Your next tour is: {dateOnly} | {startTimeOnly} - {endTimeOnly} | Nobody has made a reservation\n");
    }
    // 1 visitor expected in next tour
    public void ShowOne(DateOnly dateOnly, TimeOnly startTimeOnly, TimeOnly endTimeOnly, int amount = 1)
    {
        WriteLine($"Your next tour is: {dateOnly} | {startTimeOnly} - {endTimeOnly} | {amount} visitor has made a resevertaion\n");
    }
    // Atleast 2 visitors expected in next tour
    public void ShowMany(DateOnly dateOnly, TimeOnly startTimeOnly, TimeOnly endTimeOnly, int amount)
    {
        WriteLine($"Your next tour is: {dateOnly} | {startTimeOnly} - {endTimeOnly} | {amount} visitors have made a resevertaion\n");
    }
}