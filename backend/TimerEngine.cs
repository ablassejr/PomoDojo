using System;
using System.Runtime.InteropServices;

namespace PomoDojo.Backend
{
    /// P/Invoke declarations for the native C++ TimerEngine library
    public static class TimerEngineNative
    {
        private const string DllName = "../data/TimerEngine";
        /// Create a new TimerEngine instance
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr TE_Create();


        /// Destroy a TimerEngine instance
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Destroy(IntPtr engine);


        /// Set the session for the timer to manage
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_SetSession(IntPtr engine, IntPtr session);


        /// Start the timer loop in a background thread
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Start(IntPtr engine);


        /// Stop the timer loop and wait for thread to finish
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Stop(IntPtr engine);


        /// Check if the timer is currently running
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TE_IsRunning(IntPtr engine);

    }


    /// Provides a convenient C# interface to the C++ TimerEngine
    public class TimerEngine
    {
        private IntPtr handle;
        private bool disposed;


        /// Create a new managed TimerEngine instance

        public TimerEngine()
        {
            handle = TimerEngineNative.TE_Create();
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create native TimerEngine");
            disposed = false;
        }


        /// Set the session for the timer to manage
        public void SetSession(IntPtr sessionPtr) => TimerEngineNative.TE_SetSession(handle, sessionPtr);


        /// Start the timer loop in a background thread
        public void Start() => TimerEngineNative.TE_Start(handle);


        /// Stop the timer loop and wait for thread to finish
        public void Stop() => TimerEngineNative.TE_Stop(handle);


        /// Check if the timer is currently running
        public bool IsRunning
        {
            get => TimerEngineNative.TE_IsRunning(handle) != 0;
        }
    }
}
