public class Profile
{
    public string Username { get; set; } = "";
    public UserSettings Settings { get; set; }
    public int TotalPomodorosCompleted { get; set; } = 0;

    public Profile(string username)
    {
        Username = username;
        Settings = new UserSettings();
        TotalPomodorosCompleted = 0;
    }

    public static Profile SetupProfile()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine() ?? "User";

        Console.Write($"Focus duration in minutes (default 25): ");
        int focusMinutes = int.TryParse(Console.ReadLine(), out int f) ? f : 25;

        Console.Write($"Short break duration in minutes (default 5): ");
        int shortBreak = int.TryParse(Console.ReadLine(), out int s) ? s : 5;

        Console.Write($"Long break duration in minutes (default 15): ");
        int longBreak = int.TryParse(Console.ReadLine(), out int l) ? l : 15;

        Console.Write($"Pomodoros before long break (default 4): ");
        int pomosBeforeLong = int.TryParse(Console.ReadLine(), out int p) ? p : 4;

        Console.Write("Auto-start next session? (y/n, default y): ");
        string autoStartInput = Console.ReadLine()?.ToLower() ?? "y";
        bool autoStart = autoStartInput != "n";

        Profile profile = new Profile(username);
        profile.Settings = new UserSettings
        {
            FocusMinutes = focusMinutes,
            ShortBreakMinutes = shortBreak,
            LongBreakMinutes = longBreak,
            PomodorosBeforeLongBreak = pomosBeforeLong,
            AutoStartNext = autoStart
        };

        return profile;
    }

    public static void LoadProfile(string username, SessionManager manager)
    {
        Profile? profile = Program.storedProfiles.FirstOrDefault(p => p.Username == username);
        if (profile != null)
        {
            manager.ActiveSession.currentProfile = profile;
        }
        else
        {
            Console.WriteLine("Profile not found. Create one? (y/N): ");
            string input = Console.ReadLine()?.ToLower() ?? "n";
            if (input == "y")
            {
                Profile newProfile = SetupProfile();
                Program.storedProfiles.Append(newProfile);
                manager.ActiveSession.currentProfile = newProfile;
            }
            else if (input == "n")
            {
                Console.WriteLine("Please Try Again.");
            }

        }
    }

    public static void SaveProfileSettings(Profile profile, UserSettings settings)
    {
        Profile profileToSave = Program.storedProfiles.FirstOrDefault(p => p.Username == profile.Username);
        profileToSave!.Settings = settings;
    }
    public void IncrementPomodoros()
    {
        TotalPomodorosCompleted++;
    }
}
