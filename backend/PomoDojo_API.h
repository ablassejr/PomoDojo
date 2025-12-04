#ifdef POMODOJO_EXPORTS
#define POMODOJO_API __attribute__((dllexport))
#else
#define POMODOJO_API __attribute__((dllimport))
#endif

extern "C" {
POMODOJO_API void StartPomodojo(int workMinutes, int breakMinutes);
POMODOJO_API void StopPomodojo();
POMODOJO_API int GetRemainingSeconds();
POMODOJO_API bool IsWorkPeriod();
}
