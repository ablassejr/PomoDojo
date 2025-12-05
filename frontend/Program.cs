using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Program
{
    public static Profile[] StoredProfiles = Array.Empty<Profile>();

    public static void Main()
    {
        Directory.CreateDirectory("./data");

        if (File.Exists("./data/profiles.json"))
        {
            string content = File.ReadAllText("./data/profiles.json");
            if (!string.IsNullOrWhiteSpace(content))
            {
                try
                {
                    var profiles = JsonSerializer.Deserialize<List<Profile>>(content);
                    if (profiles != null && profiles.Count > 0)
                    {
                        StoredProfiles = profiles.ToArray();
                    }
                }
                catch (JsonException) { }
            }
        }

        UIController ui = new();
        ui.Run();
    }
}
