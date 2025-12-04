using System.Text.Json;

public class Profile
{
    public string Username { get; set; } = "";
    public UserSettings Settings { get; set; }
    public int TotalPomodorosCompleted { get; set; } = 0;

    public Profile()
    {
        Settings = new UserSettings();
    }

    public Profile(string username)
    {
        Username = username;
        Settings = new UserSettings();
        TotalPomodorosCompleted = 0;
    }

    public static void SaveAllProfiles()
    {
        try
        {
            var list = Program.storedProfiles?.ToList() ?? new List<Profile>();
            string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
            Directory.CreateDirectory("./data");
            File.WriteAllText("./data/profiles.json", json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving profiles: {ex.Message}");
        }
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

        // Save the new profile
        var list = Program.storedProfiles?.ToList() ?? new List<Profile>();
        list.Add(profile);
        Program.storedProfiles = list.ToArray();
        SaveAllProfiles();

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
                // SetupProfile now handles saving internally
                manager.ActiveSession.currentProfile = newProfile;
            }
            else
            {
                Console.WriteLine("Please Try Again.");
            }
        }
    }

    public static void SaveProfileSettings(Profile profile, UserSettings settings)
    {
        if (profile == null || Program.storedProfiles == null) return;

        Profile? profileToSave = Program.storedProfiles.FirstOrDefault(p => p.Username == profile.Username);
        if (profileToSave != null)
        {
            profileToSave.Settings = settings;
            SaveAllProfiles();
        }
    }
    public void IncrementPomodoros()
    {
        TotalPomodorosCompleted++;
    }
}
