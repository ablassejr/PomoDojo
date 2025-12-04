#define PD_API_EXPORTS
#include "PD_API.h"
#include <atomic>
#include <chrono>
#include <thread>
using namespace std;

static atomic<bool> running;    // flag indicating whether the app is running
static atomic<bool> workPeriod; // flag indicating whether it's a work period or break period
static atomic<int>
    remainingSeconds;

static thread timerThread;


void TimerLoop(int workMin, int breakMin)
{
  while (running) {
    // Work Session
    workPeriod = true;
    remainingSeconds = workMin * 60; // convert minutes to seconds

    while (running &&
           remainingSeconds > 0) // while loop to keep the work timer going
    {
      this_thread::sleep_for(chrono::seconds(1));
      remainingSeconds--;
    }

    if (!running)
      break;

    // Break Session
    workPeriod = false;
    remainingSeconds = breakMin * 60;

    while (running &&
           remainingSeconds > 0) // while loop to keep the break timer going
    {
      this_thread::sleep_for(chrono::seconds(1));
      remainingSeconds--;
    }
  }
}

extern "C" POMODOJO_API void
StartInterval(int workMinutes,
              int breakMinutes) // function to start pomodoro interval
{
  if (running)
    return;

  running = true;
  timerThread =
      thread(TimerLoop, workMinutes, breakMinutes); // async thread for timer
}

extern "C" POMODOJO_API void StopInterval() // function to stop pomodoro interval
{
  running = false;
  if (timerThread.joinable())
    timerThread.join();
}

extern "C" POMODOJO_API int
GetRemainingSeconds()
{
  return remainingSeconds;
}

extern "C" POMODOJO_API int
IsWorkPeriod()
{
  return workPeriod;
}
