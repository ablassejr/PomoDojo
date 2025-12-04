#define POMODOJO_EXPORTS
#include "SessionManager.h"
#include "TimerEngine.h"
#include <chrono>





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

  // Start the background timer thread
  timerThread = std::thread(&TimerEngine::RunTimerLoop, this);
}

void TimerEngine::Stop() {
  if (!running.load()) {
    return;
  }

  // stop signal
  stopRequested = true;


  if (timerThread.joinable()) {
    timerThread.join();
  }

  running = false;
}

bool TimerEngine::IsRunning() const { return running.load(); }


}

void TimerEngine::RunTimerLoop() {
  while (!stopRequested.load()) {
    {
      std::lock_guard<std::mutex> lock(sessionMutex);

      if (activeSession != nullptr && activeSession->Running &&
          !activeSession->Paused) {
        // Tick: decrement remaining seconds
        if (activeSession->RemainingSeconds > 0) {
          activeSession->RemainingSeconds--;
        }

        // Invoke callback if set
        if (tickCallback != nullptr) {
          tickCallback(callbackUserData);
        }
      }
    }

    // Sleep for 1 second
    std::this_thread::sleep_for(std::chrono::seconds(1));
  }
}

// C API Implementation

extern "C" {

POMODOJO_API void *TE_Create() { return new TimerEngine(); }


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


}
