using System;

public class SessionManager
{
    private UserSettings settings = new();
    private readonly NotificationService notifier = new();
    private readonly TimerEngine timer = new();

    private Session? activeSession;
    private Profile? currentProfile;
    private int completedPomodoros = 0;

    public SessionManager()
    {
        timer.Start();
    }

    public Session? ActiveSession => activeSession;
    public UserSettings Settings => settings;

    public void SetCurrentProfile(Profile profile)
    {
        currentProfile = profile;
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

            if (currentProfile != null)
            {
                currentProfile.IncrementPomodoros();
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
}
