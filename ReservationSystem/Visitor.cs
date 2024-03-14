public class Visitor
{
    public string Name { get; set; }
    public int Age { get; set; }
    public Guid VisitorId { get; private set; }
    public int RondleidingChoice { get; set; }

    public Visitor(string name, int rondleidingChoice)
    {
        Name = name;
        VisitorId = Guid.NewGuid();
        RondleidingChoice = rondleidingChoice;
    }
}
