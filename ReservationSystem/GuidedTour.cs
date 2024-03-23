public class GuidedTour
{
    private const int MaxCapacity = 13;
    private Dictionary<int, List<Visitor>> tourSlots;

    public GuidedTour()
    {
        tourSlots = new Dictionary<int, List<Visitor>>();

        // Initialize the dictionary with slots for each hour from 9 to 17 (inclusive)
        for (int hour = 9; hour <= 17; hour++)
        {
            tourSlots.Add(hour, new List<Visitor>());
        }
    }

    public void ListAvailableTours()
    {
        Console.WriteLine("Available tour times:");
        foreach (var slot in tourSlots)
        {
            Console.WriteLine($"{slot.Key}:00 - {slot.Value.Count} participants (Max {MaxCapacity})");
        }
    }

    public bool JoinTour(int hour, Visitor visitor)
    {
        // Check if the slot exists and is not full
        if (tourSlots.ContainsKey(hour) && tourSlots[hour].Count < MaxCapacity)
        {
            tourSlots[hour].Add(visitor);
            return true;
        }
        return false;
    }

    // Methods for editing or cancelling tours can work on the tourSlots dictionary directly
}
 