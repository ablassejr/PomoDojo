using System.Text.Json;

public class Program
{
    public static bool setupMode = false;
    public static Profile[] storedProfiles = Array.Empty<Profile>();

    public static void Main()
    {
        // Ensure data directory exists
        Directory.CreateDirectory("./data");

        if (File.Exists("./data/profiles.json"))
        {
            string content = File.ReadAllText("./data/profiles.json");
            if (!string.IsNullOrWhiteSpace(content))
            {
                var profiles = JsonSerializer.Deserialize<List<Profile>>(content);
                if (profiles != null && profiles.Count > 0)
                {
                    storedProfiles = profiles.ToArray();
                }
            }
        }

        UIController ui = new();
        ui.Run();
    }
}
