public class UserSettings
{
    public int FocusMinutes { get; set; } = 25;
    public int ShortBreakMinutes { get; set; } = 5;
    public int LongBreakMinutes { get; set; } = 15;
    public int PomodorosBeforeLongBreak { get; set; } = 4;
    public bool AutoStartNext { get; set; } = true;
}
