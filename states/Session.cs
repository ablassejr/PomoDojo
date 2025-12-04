public enum SessionType
{
    Focus,
    ShortBreak,
    LongBreak,
    None
}

public class Session
{
    public SessionType Type { get; private set; }
    public int DurationSeconds { get; private set; }
    public int RemainingSeconds { get; private set; }
    public Profile currentProfile { get; set; }
    public bool IsRunning { get; private set; }
    public bool IsPaused { get; private set; }

    public Session(SessionType type, int minutes)
    {
        Type = type;
        DurationSeconds = minutes * 60;
        RemainingSeconds = DurationSeconds;
        IsRunning = false;
        IsPaused = false;
    }

    public void Start()
    {
        IsRunning = true;
        IsPaused = false;
    }

    public void Pause()
    {
        if (IsRunning) IsPaused = true;
    }

    public void Resume()
    {
        if (IsRunning && IsPaused) IsPaused = false;
    }

    public void Stop()
    {
        IsRunning = false;
        IsPaused = false;
        RemainingSeconds = DurationSeconds;
    }

    public void Tick()
    {
        if (IsRunning && !IsPaused && RemainingSeconds > 0)
            RemainingSeconds--;
    }

    public bool IsFinished() => RemainingSeconds <= 0;

    public string TypeName() => Type switch
    {
        SessionType.Focus => "Focus",
        SessionType.ShortBreak => "Short Break",
        SessionType.LongBreak => "Long Break",
        _ => "None"
    };

    public string Login(SessionManager manager)
    {
        Console.Write("Enter username: ");
        string username = Console.ReadLine() ?? "User";

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
                manager.ActiveSession.currentProfile = Profile.SetupProfile();
            }
            else
            {
                Console.WriteLine("User not found.");
            }
        }
        return username;
    }
}
