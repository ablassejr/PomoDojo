using System;
using System.Threading;
using System.Threading.Tasks;

public class TimerEngine
{
    private CancellationTokenSource cts;
    private Session activeSession;

    public void SetSession(Session session)
    {
        activeSession = session;
    }

    public void Start()
    {
        cts = new CancellationTokenSource();
        _ = RunTimerLoopAsync(cts.Token);
    }

    public void Stop()
    {
        cts?.Cancel();
    }

    private async Task RunTimerLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (activeSession != null &&
                activeSession.IsRunning &&
                !activeSession.IsPaused)
            {
                activeSession.Tick();
            }
            await Task.Delay(1000);
        }
    }
}

public class SessionManager
{
    private readonly UserSettings settings = new();
    private readonly NotificationService notifier = new();
    private readonly TimerEngine timer = new();

    private Session activeSession;
    private int completedPomodoros = 0;

    public SessionManager()
    {
        timer.Start();
    }

    public Session ActiveSession => activeSession;
    public UserSettings Settings => settings;

    private void StartSession(SessionType type, int minutes)
    {
        activeSession = new Session(type, minutes);
        timer.SetSession(activeSession);
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
