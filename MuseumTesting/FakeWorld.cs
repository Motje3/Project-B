
namespace ReservationSystem;

public class FakeWorld : IWorld
{
    private DateTime? _now = null;

    public DateTime Now
    {
        get => _now ?? throw new NullReferenceException();
        set => _now = value;
    }

    public List<string> LinesWritten { get; } = new();

    public void WriteLine(string line)
    {
        LinesWritten.Add(line);
    }

    public void Write(string line)
    {
        LinesWritten.Add(line);  // keep in mind it stores it like WritLine as single string in list
    }

    public List<string> LinesToRead { private get; set; } = new();

    public string ReadLine()
    {
        string firstLine = LinesToRead.ElementAt(0);
        LinesToRead.RemoveAt(0);
        return firstLine;
    }

    public Dictionary<string, string> Files = new();

    public string ReadAllText(string path)
    {
        return Files[path];
    }

    public void WriteAllText(string path, string contents)
    {
        Files[path] = contents;
    }

    public ConsoleKeyInfo ReadKey(bool intercept)
    {

        if (!LinesToRead.Last().Contains(","))
        {
            throw new FormatException("Input is not comma seperated");
        }

        String[] unEdditedInput = LinesToRead[0].Trim(',').Split(",");
        string nextInput = unEdditedInput[0];
        unEdditedInput[0] = null;


        LinesToRead[0] = String.Join(",", unEdditedInput);

        if (LinesToRead[0] == "")
            LinesToRead.RemoveAt(0);

        ConsoleKey pressed;
        if (nextInput.Length == 1)
            pressed = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), nextInput.ToUpper());
        else
            pressed = (ConsoleKey)Enum.Parse(typeof(ConsoleKey), nextInput);
        ConsoleKeyInfo info = new('0', pressed, false, false, false);
        return info;
    }
}