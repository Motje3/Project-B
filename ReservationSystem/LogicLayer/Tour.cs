using ReservationSystem;
public class Tour
{
    public Guid TourId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime EndTime { get; private set; }
    public int Duration { get; private set; }
    public int MaxCapacity { get; private set; }
    public List<Visitor> ExpectedVisitors { get; set; } = new List<Visitor>();
    public List<Visitor> PresentVisitors { get; set; } = new List<Visitor>();
    public bool Started { get; set; }
    public bool Deleted { get; set; }
    public Guide AssignedGuide { get; set; }

    public Tour(Guid tourId, DateTime startTime, int duration, int maxCapacity, bool completed, bool deleted, Guide assignedGuide)
    {
        TourId = tourId;
        StartTime = startTime;
        EndTime = startTime.AddMinutes(duration);
        Duration = duration;
        MaxCapacity = maxCapacity;
        Started = completed;
        Deleted = deleted;
        AssignedGuide = assignedGuide;
        assignedGuide.AssignedTourIds.Add(tourId);
    }

    public void AddVisitor(Visitor visitor)
    {
        if (!Deleted && !Started && ExpectedVisitors.Count < MaxCapacity && !ExpectedVisitors.Any(v => v.TicketCode == visitor.TicketCode))
        {
            ExpectedVisitors.Add(visitor);
            TourDataManager.SaveTours();
        }
    }

    public void RemoveVisitor(Visitor visitor)
    {
        if (ExpectedVisitors.Remove(visitor))
        {
            TourDataManager.SaveTours();
        }
    }

    public void TransferVisitor(Visitor visitor, Tour targetTour)
    {
        if (visitor != null && !Deleted && !Started && targetTour != null && !targetTour.Deleted && !targetTour.Started)
        {
            RemoveVisitor(visitor);
            targetTour.AddVisitor(visitor);
        }
    }
}