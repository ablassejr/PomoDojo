#define POMODOJO_EXPORTS
#include "SessionManager.h"
#include "TimerEngine.h"
#include <chrono>

// TimerEngine implementation
// Ports the C# async timer pattern to native C++ threading



void TimerEngine::SetSession(Session *session) {
  activeSession = session;
}

void TimerEngine::Start() {
  // Prevent double-start
  if (running.load()) {
    return;
  }

  stopRequested = false;
  running = true;

  // Launch background timer thread (mirrors C# RunTimerLoopAsync)
  timerThread = std::thread(&TimerEngine::RunTimerLoop, this);
}

void TimerEngine::Stop() {
  if (!running.load()) {
    return;
  }

  // Signal stop (mirrors C# CancellationTokenSource.Cancel())
  stopRequested = true;

  // Wait for thread to finish
  if (timerThread.joinable()) {
    timerThread.join();
  }

  running = false;
}

bool TimerEngine::IsRunning() const { return running.load(); }

void TimerEngine::SetTickCallback(TickCallback callback, void *userData) {
  tickCallback = callback;
  callbackUserData = userData;
}

void TimerEngine::RunTimerLoop() {
  // Mirrors the C# pattern:
  // while (!token.IsCancellationRequested)
  // {
  //     if (activeSession != null && activeSession.IsRunning &&
  //     !activeSession.IsPaused)
  //         activeSession.Tick();
  //     await Task.Delay(1000);
  // }

  while (!stopRequested.load()) {
    {
      std::lock_guard<std::mutex> lock(sessionMutex);

      if (activeSession != nullptr && activeSession->Running &&
          !activeSession->Paused) {
        // Tick: decrement remaining seconds (mirrors C# Session.Tick())
        if (activeSession->RemainingSeconds > 0) {
          activeSession->RemainingSeconds--;
        }

        // Invoke callback if set
        if (tickCallback != nullptr) {
          tickCallback(callbackUserData);
        }
      }
    }

    // Sleep for 1 second (mirrors C# Task.Delay(1000))
    std::this_thread::sleep_for(std::chrono::seconds(1));
  }
}

// C API Implementation for P/Invoke

extern "C" {

POMODOJO_API void *TE_Create() { return new TimerEngine(); }

POMODOJO_API void TE_Destroy(void *engine) {
  if (engine) {
    delete static_cast<TimerEngine *>(engine);
  }
}

POMODOJO_API void TE_SetSession(void *engine, void *session) {
  if (engine) {
    static_cast<TimerEngine *>(engine)->SetSession(
        static_cast<Session *>(session));
  }
}

POMODOJO_API void TE_Start(void *engine) {
  if (engine) {
    static_cast<TimerEngine *>(engine)->Start();
  }
}

POMODOJO_API void TE_Stop(void *engine) {
  if (engine) {
    static_cast<TimerEngine *>(engine)->Stop();
  }
}

POMODOJO_API int TE_IsRunning(void *engine) {
  if (engine) {
    return static_cast<TimerEngine *>(engine)->IsRunning() ? 1 : 0;
  }
  return 0;
}

POMODOJO_API void TE_SetTickCallback(void *engine, TickCallback callback,
                                     void *userData) {
  if (engine) {
    static_cast<TimerEngine *>(engine)->SetTickCallback(callback, userData);
  }
}

} // extern "C"
