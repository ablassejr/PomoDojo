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

enum class SessionType {
    Focus,
    ShortBreak,
    LongBreak,
    None
};

struct UserSettings {
    int FocusMinutes = 25;
    int ShortBreakMinutes = 5;
    int LongBreakMinutes = 15;
    int PomodorosBeforeLongBreak = 4;
    bool AutoStartNext = false;
};

struct Session {
    SessionType Type;
    int DurationSeconds;
    int RemainingSeconds;
    bool Running;
    bool Paused;

    Session() : Type(SessionType::None), DurationSeconds(0), RemainingSeconds(0), Running(false), Paused(false) {}
    Session(SessionType type, int minutes) : Type(type), DurationSeconds(minutes * 60), RemainingSeconds(minutes * 60), Running(false), Paused(false) {}

    void Start() { Running = true; Paused = false; }
    void Pause() { if (Running) Paused = true; }
    void Resume() { if (Running && Paused) Paused = false; }
    void Stop() { Running = false; Paused = false; RemainingSeconds = DurationSeconds; }
    void Tick() { if (Running && !Paused && RemainingSeconds > 0) RemainingSeconds--; }
    bool IsFinished() const { return RemainingSeconds <= 0; }

    const char* TypeName() const {
        switch (Type) {
            case SessionType::Focus: return "Focus";
            case SessionType::ShortBreak: return "Short Break";
            case SessionType::LongBreak: return "Long Break";
            default: return "None";
        }
    }
};

class NotificationService {
public:
    void NotifySessionEnd(const char* sessionType);
};

class TimerEngine {
public:
    void Start();
    void Stop();
    void SetSession(Session* session);
private:
    Session* currentSession = nullptr;
};

class SessionManager {
private:
    UserSettings settings;
    NotificationService notifier;
    TimerEngine timer;
    Session* activeSession = nullptr;
    int completedPomodoros = 0;

    void StartSession(SessionType type, int minutes);

public:
    SessionManager();
    ~SessionManager();

    Session* GetActiveSession() const { return activeSession; }
    UserSettings& GetSettings() { return settings; }

    void StartFocus();
    void StartShortBreak();
    void StartLongBreak();

    void Pause();
    void Resume();
    void Stop();

    void UpdateLogic();
};

#ifdef __cplusplus
extern "C" {
#endif

POMODOJO_API void* SM_Create();
POMODOJO_API void SM_Destroy(void* manager);
POMODOJO_API void SM_StartFocus(void* manager);
POMODOJO_API void SM_StartShortBreak(void* manager);
POMODOJO_API void SM_StartLongBreak(void* manager);
POMODOJO_API void SM_Pause(void* manager);
POMODOJO_API void SM_Resume(void* manager);
POMODOJO_API void SM_Stop(void* manager);
POMODOJO_API void SM_Update(void* manager);
POMODOJO_API int SM_GetRemainingSeconds(void* manager);
POMODOJO_API int SM_GetSessionType(void* manager);
POMODOJO_API int SM_IsRunning(void* manager);
POMODOJO_API int SM_IsPaused(void* manager);

#ifdef __cplusplus
}
#endif
