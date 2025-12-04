using System.Text.Json;

public class Program
{
    public static bool setupMode = false;
    public static Profile[] storedProfiles;
    public static void Main()
    {
        if (File.Exists("./data/profiles.json") && File.ReadAllText("./data/profiles.json").Length > 0)
        {
            var profiles = JsonSerializer.Deserialize<List<Profile>>(File.ReadAllText("./data/profiles.json"));
            if (profiles != null && profiles.Count() > 0)
            {
                storedProfiles = profiles.ToArray();
            }
        }
        else if (File.Exists("./data/profiles.json"))
        {
            Program.setupMode = false;
            storedProfiles = Array.Empty<Profile>();
        }
        else
        {
            Program.setupMode = true;
            Directory.CreateDirectory("data");
            File.WriteAllText("./data/profiles.json", "");
        }

        UIController ui = new();
        ui.Run();
    }
}
