public class SessionManager : IDisposable
{
    private UserSettings settings = new();
    private readonly NotificationService notifier = new();

    private Session activeSession;
    private int completedPomodoros = 0;

    // async timer for countdown
    private CancellationTokenSource? timerCts; // cancellation signal
    private Task? timerTask;

    public Session ActiveSession => activeSession;
    public UserSettings Settings => settings;

    public SessionManager()
    {
        activeSession = new Session(SessionType.None, 0);
    }

    private void StartTimer()
    {
        StopTimer();
        timerCts = new CancellationTokenSource();
        timerTask = Task.Run(async () =>
        {
            while (!timerCts.Token.IsCancellationRequested)
            {
                if (activeSession != null && activeSession.IsRunning && !activeSession.IsPaused)
                {
                    activeSession.Tick();
                }
                try
                {
                    await Task.Delay(1000, timerCts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        });
    }

    private void StopTimer()
    {
        if (timerCts != null)
        {
            timerCts.Cancel();
            try { timerTask?.Wait(1500); } catch { }
            timerCts.Dispose();
            timerCts = null;
        }
    }

    private void StartSession(SessionType type, int minutes)
    {
        StopTimer();
        var currentProfile = activeSession?.currentProfile;
        activeSession = new Session(type, minutes);
        activeSession.currentProfile = currentProfile;  // Preserve profile reference
        activeSession.Start();
        StartTimer();
    }

    public void StartFocus() =>
        StartSession(SessionType.Focus, settings.FocusMinutes);

    public void StartShortBreak() =>
        StartSession(SessionType.ShortBreak, settings.ShortBreakMinutes);

    public void StartLongBreak() =>
        StartSession(SessionType.LongBreak, settings.LongBreakMinutes);

    public void Pause() => activeSession?.Pause();
    public void Resume() => activeSession?.Resume();

    public void Stop()
    {
        StopTimer();
        activeSession?.Stop();
    }

    public void LoadSettingsFromProfile(Profile profile)
    {
        if (profile?.Settings == null) return;
        settings = new UserSettings
        {
            FocusMinutes = profile.Settings.FocusMinutes,
            ShortBreakMinutes = profile.Settings.ShortBreakMinutes,
            LongBreakMinutes = profile.Settings.LongBreakMinutes,
            PomodorosBeforeLongBreak = profile.Settings.PomodorosBeforeLongBreak,
            AutoStartNext = profile.Settings.AutoStartNext
        };
    }

    public void UpdateLogic()
    {
        if (activeSession == null) return;
        if (!activeSession.IsFinished()) return;

        notifier.NotifySessionEnd(activeSession.TypeName());

        if (activeSession.Type == SessionType.Focus)
        {
            completedPomodoros++;

            // Save pomodoro count to profile
            if (activeSession.currentProfile != null)
            {
                activeSession.currentProfile.IncrementPomodoros();
                Profile.SaveAllProfiles();
            }

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

    public void Dispose()
    {
        StopTimer();
    }
}
