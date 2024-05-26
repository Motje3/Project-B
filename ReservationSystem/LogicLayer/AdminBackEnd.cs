using Newtonsoft.Json;
using ReservationSystem;

public static class AdminBackEnd
{
    public static void ChangeTourCapacity()
    {
        throw new NotImplementedException();
    }

    public static void ChangeTourTime()
    {
        throw new NotImplementedException();
    }

    public static string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo info = Program.World.ReadKey(true);
        ConsoleKey pressed = info.Key;
        char chosen = info.KeyChar;

        while (pressed != ConsoleKey.Enter)
        {
            if (pressed != ConsoleKey.Backspace)
            {
                PasswordChar.Show();
                password += chosen;
            }
            else if (pressed == ConsoleKey.Backspace && password.Length > 0)
            {
                Program.World.Write("\b \b"); // Moves the cursor back, writes a space to erase the star, and moves back again.
                password = password[..^1]; // Removes the last character from the password string
            }
            info = Program.World.ReadKey(true);
            pressed = info.Key;
            chosen = info.KeyChar;
        }
        Space.Show(); // Console.WriteLine(); // Ensure the cursor moves to the next line after Enter is pressed
        return password;
    }

    public static void AddNewGuidedTour()
    {
        Console.Clear();
        Console.WriteLine("Add New Guided Tour:");
        Console.WriteLine("1. Add a new guided tour for today");
        Console.WriteLine("2. Add a new guided tour to the standard schedule");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddTourForToday();
                break;
            case "2":
                AddTourToStandardSchedule();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                break;
        }
    }

    private static void AddTourForToday()
    {
        Console.Write("Enter time for the tour (hh:mm): ");
        string time = Console.ReadLine();
        DateTime tourStartTime;

        if (!DateTime.TryParse(time, out tourStartTime))
        {
            Console.WriteLine("Invalid time format. Please enter the time in hh:mm format.");
            return;
        }

        List<Guide> guides = Guide.AllGuides;
        if (guides == null || guides.Count == 0)
        {
            Console.WriteLine("No guides available.");
            return;
        }

        Console.WriteLine("Choose a guide:");
        for (int i = 0; i < guides.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {guides[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int guideChoice) || guideChoice < 1 || guideChoice > guides.Count)
        {
            Console.WriteLine("Invalid guide choice. Please select a valid guide number.");
            return;
        }

        Guide selectedGuide = guides[guideChoice - 1];
        Tour newTour = new Tour(Guid.NewGuid(), tourStartTime, 40, 20, false, false, selectedGuide);
        Tour.TodaysTours.Add(newTour);
        Tour.SaveTours();

        Console.WriteLine("Tour added successfully for today.");
    }

    private static void AddTourToStandardSchedule()
    {
        Console.Write("Enter time for the tour (hh:mm): ");
        string time = Console.ReadLine();
        DateTime tourStartTime;

        if (!DateTime.TryParse(time, out tourStartTime))
        {
            Console.WriteLine("Invalid time format. Please enter the time in hh:mm format.");
            return;
        }

        List<Guide> guides = Guide.AllGuides;
        if (guides == null || guides.Count == 0)
        {
            Console.WriteLine("No guides available.");
            return;
        }

        Console.WriteLine("Choose a guide:");
        for (int i = 0; i < guides.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {guides[i].Name}");
        }

        if (!int.TryParse(Console.ReadLine(), out int guideChoice) || guideChoice < 1 || guideChoice > guides.Count)
        {
            Console.WriteLine("Invalid guide choice. Please select a valid guide number.");
            return;
        }

        Guide selectedGuide = guides[guideChoice - 1];

        var guideAssignments = JsonConvert.DeserializeObject<List<GuideAssignment>>(File.ReadAllText(Tour.JsonGuideAssignmentsPath)) ?? new List<GuideAssignment>();
        var guideEntry = guideAssignments.FirstOrDefault(ga => ga.GuideId == selectedGuide.GuideId);

        if (guideEntry != null)
        {
            guideEntry.Tours.Add(new TourAssignment { StartTime = time });
        }
        else
        {
            guideAssignments.Add(new GuideAssignment
            {
                GuideId = selectedGuide.GuideId,  // Set GuideId property
                GuideName = selectedGuide.Name,
                Password = selectedGuide.Password,  // Set Password property
                Tours = new List<TourAssignment> { new TourAssignment { StartTime = time } }
            });
        }

        File.WriteAllText(Tour.JsonGuideAssignmentsPath, JsonConvert.SerializeObject(guideAssignments, Formatting.Indented));
        Tour.CreateToursForToday();

        Console.WriteLine("Tour added successfully to the standard schedule.");
    }


    public class GuideAssignment
    {
        public Guid GuideId { get; set; }  // Change this to Guid to match GuideId in Guide class
        public string GuideName { get; set; }
        public string Password { get; set; }
        public List<TourAssignment> Tours { get; set; } = new List<TourAssignment>();
    }


    public class TourAssignment
    {
        public string StartTime { get; set; }
    }

}