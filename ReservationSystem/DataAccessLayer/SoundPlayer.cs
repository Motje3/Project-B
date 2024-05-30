using System.Media;

namespace ReservationSystem;

public static class SoundsPlayer
{
    public static string SoundsFolderPath = "./Sounds/";

    public static void PlaySound(SoundFile soundFile)
    {
        // Early return for system tests
        if (Program.World is not RealWorld)
            return;
        
        try
        {
            SoundPlayer player = new SoundPlayer($"./Sounds/{GetSoundFileName(soundFile)}");
            using (player)
            {
                player.Load(); // Explicitly load the file to catch errors
                player.Play(); // Play the sound synchronously
            }
        }
        catch (FileNotFoundException)
        {
            throw new Exception($"No file with name {GetSoundFileName(soundFile)} has been found");
        }
    }

    public enum SoundFile
    {
        ChceckIn,
        WrongInput
    }

    private static string GetSoundFileName(SoundFile soundFile) => soundFile switch
    {
        SoundFile.ChceckIn => "CheckIn.wav",
        SoundFile.WrongInput => "WrongInput.wav",
    };
}