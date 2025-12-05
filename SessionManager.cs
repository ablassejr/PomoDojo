using System;
using PomoDojo.Interop;
//class to manage session state and notify events
public class SessionManager
{
    private UserSettings settings = new();
    private readonly NotificationService notifier = new();

    private Profile? currentProfile;
    private int completedPomodoros = 0;
    private bool isRunning = false;
    private bool wasWorkPeriod = true;
    private int lastRemainingSeconds = -1;


    public UserSettings Settings => settings;
    public bool IsRunning => isRunning;

    // expose timer state
    public int RemainingSeconds => isRunning ? NativeApi.GetRemainingSeconds() : 0;
    public bool IsWorkPeriod => isRunning ? NativeApi.IsWorkPeriod() : true;

    public string CurrentSessionType => IsWorkPeriod ? "Focus" : "Short Break";

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

    public void StartFocus()
    {
        if (isRunning) return;

        NativeApi.StartPomodojo(settings.FocusMinutes, settings.ShortBreakMinutes);
        isRunning = true;
        wasWorkPeriod = true;
        lastRemainingSeconds = settings.FocusMinutes * 60;
    }

    public void Stop()
    {
        if (!isRunning) return;

        NativeApi.StopPomodojo();
        isRunning = false;
    }
    //function to transition between work and break periods
    public void UpdateLogic()
    {
        if (!isRunning) return;

        bool currentWorkPeriod = NativeApi.IsWorkPeriod();
        int currentRemaining = NativeApi.GetRemainingSeconds();

        // break transition
        if (wasWorkPeriod && !currentWorkPeriod)
        {
            // event trigger to transition to break
            notifier.NotifySessionEnd("Focus");
            completedPomodoros++;

            if (currentProfile != null)
            {
                currentProfile.IncrementPomodoros();
                Profile.SaveAllProfiles();
            }

            // long break check
            if (completedPomodoros >= settings.PomodorosBeforeLongBreak)
            {
                completedPomodoros = 0;
            }
        }
        // work transition
        else if (!wasWorkPeriod && currentWorkPeriod)
        {
            // event trigger to transition to work
            notifier.NotifySessionEnd("Break");
        }

        wasWorkPeriod = currentWorkPeriod;
        lastRemainingSeconds = currentRemaining;
    }
    //expose pomodoro count
    public int CompletedPomodoros => completedPomodoros;
}
