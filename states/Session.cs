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
    public Profile? currentProfile { get; set; }
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
}
