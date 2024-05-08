namespace ReservationSystem;

public class RealWorld : IWorld
{
    public DateTime Now
    {
        get => DateTime.Now;
    }

    public void WriteLine(string line)
    {
        Console.WriteLine(line);
    }

    public void Write(string line)
    {
        Console.Write(line);
    }

    public string ReadLine()
    {
        return Console.ReadLine();
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }

    public void WriteAllText(string path, string contents)
    {
        File.WriteAllText(path, contents);
    }
}