namespace ReservationSystem;

// ALWAYS MAKE SURE EVRY CLASS IN VIEW (PRESENTATION LAYER) HAS INHERTECNCE OF VIEW

// FROM NOW ON ONLY USE WriteLine() INSTEAD OF Program.World.WriteLine() for evrything!

public class View
{
    public static void WriteLine(string line)
    {
        Program.World.WriteLine(line);
    }

    public static void Write(string line)
    {
        Program.World.Write(line);
    }

    public static string ReadLine()
    {
        return Program.World.ReadLine();
    }
}

// ALWAYS MAKE SURE EVRY CLASS IN VIEW (PRESENTATION LAYER) HAS INHERTECNCE OF VIEW

// FROM NOW ON ONLY USE WriteLine() INSTEAD OF Program.World.WriteLine() for evrything!