public class Program
{
  public static Profile
    public static void Main()
    {
        if (!File.Exists("data/profiles.json"))
        {
            Directory.CreateDirectory("data");
            File.WriteAllText("data/profiles.json", "");
        }
        
        UIController ui = new();
        ui.Run();
    }
}
