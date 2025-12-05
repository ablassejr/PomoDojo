using System;

public class UIController
{
    private readonly SessionManager manager = new();
    private Profile? currentProfile;

    public void Run()
    {
        Console.Clear();
        Console.WriteLine("=== POMODOJO ===\n");

        while (currentProfile == null)
        {
            Console.WriteLine("[1] Login");
            Console.WriteLine("[2] Create New Profile");
            Console.Write("Choose: ");
            string? choice = Console.ReadLine()?.Trim();

            if (choice == "1")
            {
                Console.Write("Enter username: ");
                string username = Console.ReadLine()?.Trim() ?? "";
                currentProfile = Profile.LoadProfile(username);

                if (currentProfile != null)
                {
                    Console.WriteLine($"Welcome back, {currentProfile.Username}!");
                    manager.LoadSettingsFromProfile(currentProfile);
                }
                else
                {
                    Console.WriteLine("Profile not found. Try again or create a new profile.\n");
                }
            }
            else if (choice == "2")
            {
                currentProfile = Profile.SetupProfile();
                Console.WriteLine($"Profile created! Welcome, {currentProfile.Username}!");
                manager.LoadSettingsFromProfile(currentProfile);
            }
        }

        manager.SetCurrentProfile(currentProfile);

        bool exit = false;
        while (!exit)
        {
            manager.UpdateLogic();
            DisplaySessionUI();
            ShowMenu();

            string? input = Console.ReadLine()?.Trim();

            if (input == "9") exit = true;
            else HandleChoice(input ?? "");
        }
        manager.Stop();
    }

    private void DisplaySessionUI()
    {
        if (!manager.IsRunning) return;

        int sec = manager.RemainingSeconds;
        int m = sec / 60;
        int s2 = sec % 60;

        Console.WriteLine("\n======== CURRENT SESSION ========");
        Console.WriteLine($"Type: {manager.CurrentSessionType}");
        Console.WriteLine($"Time Left: {(m < 10 ? "0" : "")}{m}:{(s2 < 10 ? "0" : "")}{s2}");
        Console.WriteLine($"Completed Pomodoros: {manager.CompletedPomodoros}");
        Console.WriteLine("==================================");
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n===== POMODOJO =====");
        Console.WriteLine("[1] Start Focus");
        Console.WriteLine("[2] Stop");
        Console.WriteLine("[3] Settings (View Only)");
        Console.WriteLine("[4] Edit Settings");
        Console.WriteLine("[9] Exit");
        Console.Write("Choose: ");
    }

    private void HandleChoice(string choice)
    {
        switch (choice)
        {
            case "1":
                manager.StartFocus();
                break;
            case "2":
                manager.Stop();
                break;
            case "3":
                ShowSettings();
                break;
            case "4":
                EditSettings();
                break;
            default:
                Console.WriteLine("Invalid choice.");
                break;
        }
    }

    private void ShowSettings()
    {
        var s = manager.Settings;
        Console.WriteLine("\nCurrent Settings:");
        Console.WriteLine($"Focus Minutes:              {s.FocusMinutes}");
        Console.WriteLine($"Short Break Minutes:        {s.ShortBreakMinutes}");
        Console.WriteLine($"Long Break Minutes:         {s.LongBreakMinutes}");
        Console.WriteLine($"Pomodoros Before Long Break:{s.PomodorosBeforeLongBreak}");
        Console.WriteLine($"Auto-Start Next:            {(s.AutoStartNext ? "ON" : "OFF")}");
    }

    private void EditSettings()
    {
        var s = manager.Settings;

        Console.WriteLine("\n--- Edit Settings ---");

        s.FocusMinutes = ReadIntWithDefault(
            $"Focus Minutes ({s.FocusMinutes}): ",
            s.FocusMinutes);

        s.ShortBreakMinutes = ReadIntWithDefault(
            $"Short Break Minutes ({s.ShortBreakMinutes}): ",
            s.ShortBreakMinutes);

        s.LongBreakMinutes = ReadIntWithDefault(
            $"Long Break Minutes ({s.LongBreakMinutes}): ",
            s.LongBreakMinutes);

        s.PomodorosBeforeLongBreak = ReadIntWithDefault(
            $"Pomodoros Before Long Break ({s.PomodorosBeforeLongBreak}): ",
            s.PomodorosBeforeLongBreak);

        Console.Write($"Auto-Start Next ({(s.AutoStartNext ? "Y" : "N")}) [Y/N, Enter = keep]: ");
        string? autoInput = Console.ReadLine()?.Trim().ToUpper();
        if (autoInput == "Y") s.AutoStartNext = true;
        else if (autoInput == "N") s.AutoStartNext = false;

        Profile.SaveProfileSettings(currentProfile, s);
        Console.WriteLine("Settings updated and saved.");
    }

    private int ReadIntWithDefault(string prompt, int currentValue)
    {
        Console.Write(prompt);
        string? input = Console.ReadLine()?.Trim();

        if (string.IsNullOrEmpty(input))
            return currentValue;

        if (int.TryParse(input, out int value) && value > 0)
            return value;

        Console.WriteLine("Invalid input, keeping previous value.");
        return currentValue;
    }
}
