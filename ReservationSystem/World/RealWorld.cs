﻿namespace ReservationSystem;

public class RealWorld : IWorld
{
    public DateTime Today
    {
        get => DateTime.Today;
    }

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

    public ConsoleKeyInfo ReadKey(bool intercept)
    {
        return Console.ReadKey(true);
    }

    public bool Exists(string path)
    {
        return File.Exists(path);
    }
}