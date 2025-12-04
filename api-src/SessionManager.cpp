#include "SessionManager.h"
#include <iostream>

// NotificationService implementation
void NotificationService::NotifySessionEnd(const char* sessionType) {
    std::cout << "Session ended: " << sessionType << std::endl;
}

// TimerEngine implementation
void TimerEngine::Start() {
    // Timer loop handled externally or via threading
}

void TimerEngine::Stop() {
    // Stop timer loop
}

void TimerEngine::SetSession(Session* session) {
    currentSession = session;
}

// SessionManager implementation
SessionManager::SessionManager() {
    timer.Start();
}

SessionManager::~SessionManager() {
    if (activeSession) {
        delete activeSession;
        activeSession = nullptr;
    }
    timer.Stop();
}

void SessionManager::StartSession(SessionType type, int minutes) {
    if (activeSession) {
        delete activeSession;
    }
    activeSession = new Session(type, minutes);
    timer.SetSession(activeSession);
    activeSession->Start();
}

void SessionManager::StartFocus() {
    StartSession(SessionType::Focus, settings.FocusMinutes);
}

void SessionManager::StartShortBreak() {
    StartSession(SessionType::ShortBreak, settings.ShortBreakMinutes);
}

void SessionManager::StartLongBreak() {
    StartSession(SessionType::LongBreak, settings.LongBreakMinutes);
}

void SessionManager::Pause() {
    if (activeSession) {
        activeSession->Pause();
    }
}

void SessionManager::Resume() {
    if (activeSession) {
        activeSession->Resume();
    }
}

void SessionManager::Stop() {
    if (activeSession) {
        activeSession->Stop();
    }
}

void SessionManager::UpdateLogic() {
    if (activeSession == nullptr) return;
    if (!activeSession->IsFinished()) return;

    notifier.NotifySessionEnd(activeSession->TypeName());

    if (activeSession->Type == SessionType::Focus) {
        completedPomodoros++;

        if (completedPomodoros >= settings.PomodorosBeforeLongBreak) {
            completedPomodoros = 0;
            if (settings.AutoStartNext) StartLongBreak();
            return;
        }

        if (settings.AutoStartNext) StartShortBreak();
    } else {
        if (settings.AutoStartNext) StartFocus();
    }
}

// C API implementation
extern "C" {

POMODOJO_API void* SM_Create() {
    return new SessionManager();
}

POMODOJO_API void SM_Destroy(void* manager) {
    if (manager) {
        delete static_cast<SessionManager*>(manager);
    }
}

POMODOJO_API void SM_StartFocus(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->StartFocus();
    }
}

POMODOJO_API void SM_StartShortBreak(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->StartShortBreak();
    }
}

POMODOJO_API void SM_StartLongBreak(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->StartLongBreak();
    }
}

POMODOJO_API void SM_Pause(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->Pause();
    }
}

POMODOJO_API void SM_Resume(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->Resume();
    }
}

POMODOJO_API void SM_Stop(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->Stop();
    }
}

POMODOJO_API void SM_Update(void* manager) {
    if (manager) {
        static_cast<SessionManager*>(manager)->UpdateLogic();
    }
}

POMODOJO_API int SM_GetRemainingSeconds(void* manager) {
    if (manager) {
        Session* session = static_cast<SessionManager*>(manager)->GetActiveSession();
        if (session) {
            return session->RemainingSeconds;
        }
    }
    return 0;
}

POMODOJO_API int SM_GetSessionType(void* manager) {
    if (manager) {
        Session* session = static_cast<SessionManager*>(manager)->GetActiveSession();
        if (session) {
            return static_cast<int>(session->Type);
        }
    }
    return -1;
}

POMODOJO_API int SM_IsRunning(void* manager) {
    if (manager) {
        Session* session = static_cast<SessionManager*>(manager)->GetActiveSession();
        if (session) {
            return session->Running ? 1 : 0;
        }
    }
    return 0;
}

POMODOJO_API int SM_IsPaused(void* manager) {
    if (manager) {
        Session* session = static_cast<SessionManager*>(manager)->GetActiveSession();
        if (session) {
            return session->Paused ? 1 : 0;
        }
    }
    return 0;
}

}
