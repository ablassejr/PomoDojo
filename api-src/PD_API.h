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

#ifdef __cplusplus
extern "C" {
#endif

POMODOJO_API void StartInterval(int workMinutes, int breakMinutes);
POMODOJO_API void StopInterval();
POMODOJO_API int GetRemainingSeconds();
POMODOJO_API int IsWorkPeriod();

#ifdef __cplusplus
}
#endif
