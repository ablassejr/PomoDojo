#pragma once

#ifdef _WIN32
#ifdef POMODOJO_EXPORTS
#define POMODOJO_API __declspec(dllexport)
#else
#define POMODOJO_API __declspec(dllimport)
#endif
#else
#ifdef POMODOJO_EXPORTS
#define POMODOJO_API __attribute__((visibility("default")))
#else
#define POMODOJO_API
#endif
#endif

#include <atomic>
#include <functional>
#include <mutex>
#include <thread>
// Forward declaration for Session pointer compatibility
struct Session;

// Callback type for tick notifications (called each second when running)
using TickCallback = void (*)(void *userData);

// TimerEngine - Autonomous timer that runs a background thread
class TimerEngine {
public:
  TimerEngine();
  ~TimerEngine();

  // Prevent copying (thread ownership)
  TimerEngine(const TimerEngine &) = delete;
  TimerEngine &operator=(const TimerEngine &) = delete;

  // Set the session to be ticked by this timer
  void SetSession(Session *session);

  // Start the timer loop (runs in background thread)
  void Start();

  // Stop the timer loop and wait for thread to finish
  void Stop();

  // Check if timer is currently running
  bool IsRunning() const;



private:
  void RunTimerLoop();

  Session *activeSession;
  std::atomic<bool> running;
  std::atomic<bool> stopRequested;
  std::thread timerThread;
  std::mutex sessionMutex;

  TickCallback tickCallback;
  void *callbackUserData;
};

#ifdef __cplusplus
extern "C" {
#endif

// C API for TimerEngine
POMODOJO_API void *TE_Create();
POMODOJO_API void TE_SetSession(void *engine, void *session);
POMODOJO_API void TE_Start(void *engine);
POMODOJO_API void TE_Stop(void *engine);
POMODOJO_API int TE_IsRunning(void *engine);


#ifdef __cplusplus
}
#endif
