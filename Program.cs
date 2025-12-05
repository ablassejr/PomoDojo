using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

public class Program
{
    // list to hold saved profiles
    public static Profile[] StoredProfiles = Array.Empty<Profile>();

    public static void Main()
    {
        Directory.CreateDirectory("./data"); // Create directory if it doesn't exist

        if (File.Exists("./data/profiles.json"))
        {
            string content = File.ReadAllText("./data/profiles.json"); // read profile data
            var profiles = JsonSerializer.Deserialize<List<Profile>>(content); // parse json data
            if (profiles != null && profiles.Count > 0)
                StoredProfiles = profiles.ToArray(); // convert parsed json data to array
        }

        UIController ui = new();
        ui.Run();
    }
}
