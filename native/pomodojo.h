#pragma once

#ifdef _WIN32
    #ifdef POMODOJO_EXPORTS
        #define POMODOJO_API __declspec(dllexport)
    #else
        #define POMODOJO_API __declspec(dllimport)
    #endif
#else
    #define POMODOJO_API __attribute__((visibility("default")))
#endif

extern "C" {
    POMODOJO_API void StartPomodojo(int workMinutes, int breakMinutes);
    POMODOJO_API void StopPomodojo();
    POMODOJO_API int GetRemainingSeconds();
    POMODOJO_API bool IsWorkPeriod();
}
