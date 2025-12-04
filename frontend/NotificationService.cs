using System;

public class NotificationService
{
    public void NotifySessionEnd(string sessionName)
    {
        Console.WriteLine($"\n>>> {sessionName} session finished! \a");
    }
}
