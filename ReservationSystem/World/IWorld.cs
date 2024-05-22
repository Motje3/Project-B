namespace ReservationSystem;

public interface IWorld
{
    DateTime Today { get; }
    DateTime Now { get; }
    void WriteLine(string line);

    void Write(string line);

    string ReadLine();
    ConsoleKeyInfo ReadKey(bool intercept);

    string ReadAllText(string path);

    void WriteAllText(string path, string contents);
}