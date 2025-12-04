#pragma once

#ifdef POMODOJO_EXPORTS
#define POMODOJO_API _declspec(dllexport)
#else
#define POMODOJO_API _declspec(dllimport)
#endif

extern "C" {
	POMODOJO_API void StartPomodojo(int workMinutes, int breakMinutes);
	POMODOJO_API void StopPomodojo();
	POMODOJO_API int GetRemainingSeconds();
	POMODOJO_API bool IsWorkPeriod();

	
}
