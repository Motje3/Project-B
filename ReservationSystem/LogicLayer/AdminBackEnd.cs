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
        AdminMessages.ShowAddNewGuidedTourOptions();

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddTourForTomorrow();
                break;
            case "2":
                AddTourToStandardSchedule();
                break;
            default:
                AdminMessages.ShowInvalidOption();
                WaitForUser();
                break;
        }
    }

    public static void AddTourForTomorrow()
    {
        AdminMessages.ShowEnterTimePrompt();
        string time = Console.ReadLine();
        DateTime tourStartTime;

        if (!DateTime.TryParse(time, out tourStartTime))
        {
            AdminMessages.ShowInvalidTimeFormat();
            WaitForUser();
            return;
        }

        List<Guide> guides = Guide.AllGuides;
        if (guides == null || guides.Count == 0)
        {
            AdminMessages.ShowNoGuidesAvailable();
            WaitForUser();
            return;
        }

        var uniqueGuides = guides.GroupBy(g => g.GuideId).Select(g => g.First()).ToList();
        AdminMessages.ShowChooseGuide(uniqueGuides);

        if (!int.TryParse(Console.ReadLine(), out int guideChoice) || guideChoice < 1 || guideChoice > uniqueGuides.Count)
        {
            AdminMessages.ShowInvalidGuideChoice();
            WaitForUser();
            return;
        }

        Guide selectedGuide = uniqueGuides[guideChoice - 1];
        Tour newTour = new Tour(Guid.NewGuid(), tourStartTime, 40, 13, false, false, selectedGuide);
        TourTools.TodaysTours.Add(newTour);
        TourDataManager.SaveTours();
        try { Console.Clear(); } catch { }

        AdminMessages.ShowTourAddedSuccessfully();
        WaitForUser();
    }

    public static void AddTourToStandardSchedule()
    {
        AdminMessages.ShowEnterTimePrompt();
        string time = Console.ReadLine();
        DateTime tourStartTime;

        if (!DateTime.TryParse(time, out tourStartTime))
        {
            AdminMessages.ShowInvalidTimeFormat();
            WaitForUser();
            return;
        }

        string amPmTime = tourStartTime.ToString("hh:mm tt"); // Convert to AM/PM format

        List<Guide> guides = Guide.AllGuides;
        if (guides == null || guides.Count == 0)
        {
            AdminMessages.ShowNoGuidesAvailable();
            WaitForUser();
            return;
        }

        AdminMessages.ShowChooseGuide(guides);

        if (!int.TryParse(Console.ReadLine(), out int guideChoice) || guideChoice < 1 || guideChoice > guides.Count)
        {
            AdminMessages.ShowInvalidGuideChoice();
            WaitForUser();
            return;
        }

        Guide selectedGuide = guides[guideChoice - 1];

        var guideAssignments = JsonConvert.DeserializeObject<List<GuideAssignment>>(File.ReadAllText(TourTools.JsonGuideAssignmentsPath)) ?? new List<GuideAssignment>();
        var guideEntry = guideAssignments.FirstOrDefault(ga => ga.GuideId == selectedGuide.GuideId);

        if (guideEntry != null)
        {
            guideEntry.Tours.Add(new TourAssignment { StartTime = amPmTime });
        }
        else
        {
            guideAssignments.Add(new GuideAssignment
            {
                GuideId = selectedGuide.GuideId,
                GuideName = selectedGuide.Name,
                Password = selectedGuide.Password,
                Tours = new List<TourAssignment> { new TourAssignment { StartTime = amPmTime } }
            });
        }

        File.WriteAllText(TourTools.JsonGuideAssignmentsPath, JsonConvert.SerializeObject(guideAssignments, Formatting.Indented));
        TourDataManager.CreateToursForToday();
        try { Console.Clear(); } catch { }

        AdminMessages.ShowTourAddedSuccessfully();
        WaitForUser();
    }

    private static void WaitForUser()
    {
        Thread.Sleep(2000); // Pause for 2 seconds
        AdminMessages.ShowWaitForUser();

        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Spacebar)
        {
            keyInfo = Console.ReadKey(true);
        }

        if (keyInfo.Key == ConsoleKey.Enter)
        {
            try { Console.Clear(); } catch { }
        }
    }

    public class GuideAssignment
    {
        public Guid GuideId { get; set; }
        public string GuideName { get; set; }
        public string Password { get; set; }
        public List<TourAssignment> Tours { get; set; } = new List<TourAssignment>();
    }

    public class TourAssignment
    {
        public string StartTime { get; set; }
    }
}
