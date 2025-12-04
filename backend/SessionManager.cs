public class SessionManager
{
    private readonly UserSettings settings = new();
    private readonly NotificationService notifier = new();
    private readonly PomoDojo.Backend.TimerEngine timer = new();

    private Session activeSession;

    private int completedPomodoros = 0;
    public Session ActiveSession => activeSession;
    public UserSettings Settings => settings;
    private void StartSession(SessionType type, int minutes)
    {
        activeSession = new Session(type, minutes);
        activeSession.Start();
    }

    public void StartFocus() =>
        StartSession(SessionType.Focus, settings.FocusMinutes);

    public void StartShortBreak() =>
        StartSession(SessionType.ShortBreak, settings.ShortBreakMinutes);

    public void StartLongBreak() =>
        StartSession(SessionType.LongBreak, settings.LongBreakMinutes);

    public void Pause() => activeSession?.Pause();
    public void Resume() => activeSession?.Resume();
    public void Stop() => activeSession?.Stop();
    public void Start() => activeSession?.Start();

    public void UpdateLogic()
    {
        if (activeSession == null) return;
        if (!activeSession.IsFinished()) return;

        notifier.NotifySessionEnd(activeSession.TypeName());

        if (activeSession.Type == SessionType.Focus)
        {
            completedPomodoros++;

            if (completedPomodoros >= settings.PomodorosBeforeLongBreak)
            {
                completedPomodoros = 0;
                if (settings.AutoStartNext) StartLongBreak();
                return;
            }

            if (settings.AutoStartNext) StartShortBreak();
        }
        else
        {
            if (settings.AutoStartNext) StartFocus();
        }
    }
}
