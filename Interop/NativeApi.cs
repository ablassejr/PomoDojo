using System;
using System.Runtime.InteropServices;

namespace PomoDojo.Interop
{
    public static class NativeApi
    {
        private const string DllName = "pomodojo";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void StartPomodojo(int workMinutes, int breakMinutes);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void StopPomodojo();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int GetRemainingSeconds();

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.I1)]
        public static extern bool IsWorkPeriod();
    }
}
