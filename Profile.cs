using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public class Profile
{
    public string Username { get; set; } = "";
    public UserSettings Settings { get; set; } = new();
    public int TotalPomodorosCompleted { get; set; } = 0;

    public Profile() { }

    public Profile(string username)
    {
        Username = username;
        Settings = new UserSettings();
    }

    public void IncrementPomodoros()
    {
        TotalPomodorosCompleted++;
    }

    public static void SaveAllProfiles()
    {
        var list = Program.StoredProfiles?.ToList() ?? new List<Profile>();
        string json = JsonSerializer.Serialize(list, new JsonSerializerOptions { WriteIndented = true });
        Directory.CreateDirectory("./data");
        File.WriteAllText("./data/profiles.json", json);
    }

    public static Profile SetupProfile()
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine()?.Trim() ?? "User";

        Console.Write("Focus duration in minutes (default 25): ");
        int focusMinutes = int.TryParse(Console.ReadLine(), out int f) && f > 0 ? f : 25;

        Console.Write("Short break duration in minutes (default 5): ");
        int shortBreak = int.TryParse(Console.ReadLine(), out int s) && s > 0 ? s : 5;

        Console.Write("Long break duration in minutes (default 15): ");
        int longBreak = int.TryParse(Console.ReadLine(), out int l) && l > 0 ? l : 15;

        Console.Write("Pomodoros before long break (default 4): ");
        int pomosBeforeLong = int.TryParse(Console.ReadLine(), out int p) && p > 0 ? p : 4;

        Console.Write("Auto-start next session? (y/n, default y): ");
        string autoStartInput = Console.ReadLine()?.ToLower() ?? "y";
        bool autoStart = autoStartInput != "n";

        Profile profile = new Profile(username)
        {
            Settings = new UserSettings
            {
                FocusMinutes = focusMinutes,
                ShortBreakMinutes = shortBreak,
                LongBreakMinutes = longBreak,
                PomodorosBeforeLongBreak = pomosBeforeLong,
                AutoStartNext = autoStart
            }
        };

        var list = Program.StoredProfiles?.ToList() ?? new List<Profile>();
        list.Add(profile);
        Program.StoredProfiles = list.ToArray();
        SaveAllProfiles();

        return profile;
    }

    public static Profile? LoadProfile(string username)
    {
        return Program.StoredProfiles?.FirstOrDefault(p =>
            p.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
    }

    public static void SaveProfileSettings(Profile? profile, UserSettings settings)
    {
        if (profile == null || Program.StoredProfiles == null) return;

        var profileToSave = Program.StoredProfiles.FirstOrDefault(p => p.Username == profile.Username);
        if (profileToSave != null)
        {
            profileToSave.Settings = settings;
            SaveAllProfiles();
        }
    }
}
