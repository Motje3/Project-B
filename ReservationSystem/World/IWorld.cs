namespace ReservationSystem;

public interface IWorld
{
    DateTime Now { get; }

    void WriteLine(string line);

    string ReadLine();

    string ReadAllText(string path);

    void WriteAllText(string path, string contents);
}