using System;
using System.Runtime.InteropServices;

namespace PomoDojo.Backend
{
    // API declarations
    public static class TimerEngineNative
    {
        private const string DllName = "../data/TimerEngine";
        // Create a new TimerEngine instance
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TE_Create();




        // Set the session for the timer to manage
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_SetSession(IntPtr engine, IntPtr session);


        // Start the timer loop in a background thread
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Start(IntPtr engine);


        // Stop the timer loop and wait for thread to finish
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Stop(IntPtr engine);


        // Check if the timer is currently running
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TE_IsRunning(IntPtr engine);

    }



    public class TimerEngine
    {
        private IntPtr handle;
        private bool disposed;


        // Create a new TimerEngine instance

        public TimerEngine()
        {
            handle = TimerEngineNative.TE_Create();
            disposed = false;
        }


        // Set the session for the timer to manage
        public void SetSession(IntPtr sessionPtr) => TimerEngineNative.TE_SetSession(handle, sessionPtr);


        // Start the timer loop in a background thread
        public void Start() => TimerEngineNative.TE_Start(handle);


        // Stop the timer loop and wait for thread to finish
        public void Stop() => TimerEngineNative.TE_Stop(handle);


        // Check if the timer is currently running
        public bool IsRunning
        {
            get => TimerEngineNative.TE_IsRunning(handle) != 0;
        }
    }
}
