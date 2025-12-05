#include "pomodojo.h"
#include <atomic>
#include <thread>
#include <chrono>

using namespace std;

static atomic<bool> running;      // bool to signal if the app is running
static atomic<bool> workPeriod;   // bool to signal a work period or break period
static atomic<int> remainingSeconds; // integer for the remaining seconds in the session

static thread timerThread;

void TimerLoop(int workMin, int breakMin) // function to create the timer loop for work and break session
{
    while (running)
    {
        // Work Session
        workPeriod = true;
        remainingSeconds = workMin * 60; // convert minutes to seconds

        while (running && remainingSeconds > 0) // while loop to keep the work timer going
        {
            this_thread::sleep_for(chrono::seconds(1));
            remainingSeconds--;
        }

        if (!running)
            break;

        // Break Session
        workPeriod = false;
        remainingSeconds = breakMin * 60;

        while (running && remainingSeconds > 0) // while loop to keep the break timer going
        {
            this_thread::sleep_for(chrono::seconds(1));
            remainingSeconds--;
        }
    }
}

extern "C" POMODOJO_API void StartPomodojo(int workMinutes, int breakMinutes) // function to start PomoDojo session
{
    if (running) return;

    running = true;
    timerThread = thread(TimerLoop, workMinutes, breakMinutes); // create the timer thread
}

extern "C" POMODOJO_API void StopPomodojo() // function to stop PomoDojo session
{
    running = false;
    if (timerThread.joinable())
        timerThread.join();
}

extern "C" POMODOJO_API int GetRemainingSeconds() // function that returns the remaining seconds in the session
{
    return remainingSeconds;
}

extern "C" POMODOJO_API bool IsWorkPeriod() // function to return if work session or not
{
    return workPeriod;
}
