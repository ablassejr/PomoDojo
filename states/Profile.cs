
public static class Profile
{
    public string Username { get; set; }
    public UserSettings Settings { get; set; }
    public int TotalPomodorosCompleted { get; set; }

    public UserProfile(string username)
    {
        Username = username;
        Settings = new UserSettings();
        TotalPomodorosCompleted = 0;
    }

    public void IncrementPomodoros()
    {
        TotalPomodorosCompleted++;
    }
}
