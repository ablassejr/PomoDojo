using System;
using System.Threading;
using System.Threading.Tasks;

public class TimerEngine
{
    private CancellationTokenSource? cts;
    private Session? activeSession;

    public void SetSession(Session session)
    {
        activeSession = session;
    }

    public void Start()
    {
        cts = new CancellationTokenSource();
        _ = RunTimerLoopAsync(cts.Token);
    }

    public void Stop()
    {
        cts?.Cancel();
    }

    private async Task RunTimerLoopAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            if (activeSession != null &&
                activeSession.IsRunning &&
                !activeSession.IsPaused)
            {

                activeSession.Tick();
            }
            await Task.Delay(1000);
        }
    }
}
