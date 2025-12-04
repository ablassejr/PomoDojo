using System;
using System.Runtime.InteropServices;

namespace PomoDojo.Backend
{
    /// P/Invoke declarations for the native C++ TimerEngine library
    public static class TimerEngineNative
    {
        private const string DllName = "TimerEngine";
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
        
        /// <param name="engine">Handle to the TimerEngine instance</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Start(IntPtr engine);

        
        /// Stop the timer loop and wait for thread to finish
        
        /// <param name="engine">Handle to the TimerEngine instance</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_Stop(IntPtr engine);

        
        /// Check if the timer is currently running
        
        /// <param name="engine">Handle to the TimerEngine instance</param>
        /// <returns>1 if running, 0 if stopped</returns>
k        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int TE_IsRunning(IntPtr engine);

        
        /// Set a callback to be invoked on each timer tick
        
        /// <param name="engine">Handle to the TimerEngine instance</param>
        /// <param name="callback">Callback function (or null to clear)</param>
        /// <param name="userData">User data passed to callback</param>
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TE_SetTickCallback(IntPtr engine, TickCallback? callback, IntPtr userData);

        #endregion
    }

    
    /// Managed wrapper for the native TimerEngine
    /// Provides a convenient C# interface to the C++ TimerEngine
    /// Mirrors the functionality of frontend/TimerEngine.cs but using native C++ implementation
    
    public class TimerEngine
    {
        private IntPtr handle;
        private bool disposed;
        private TimerEngineNative.TickCallback? tickCallbackHolder;

        
        /// Create a new managed TimerEngine instance
        
        public TimerEngine()
        {
            handle = TimerEngineNative.TE_Create();
            if (handle == IntPtr.Zero)
                throw new InvalidOperationException("Failed to create native TimerEngine");
            disposed = false;
        }

        
        /// Set the session for the timer to manage
        /// Mirrors: public void SetSession(Session session)
        
        /// <param name="sessionPtr">Pointer to the native Session struct</param>
        public void SetSession(IntPtr sessionPtr) => TimerEngineNative.TE_SetSession(handle, sessionPtr);

        
        /// Start the timer loop in a background thread
        /// Mirrors: public void Start()
        
        public void Start() => TimerEngineNative.TE_Start(handle);

        
        /// Stop the timer loop and wait for thread to finish
        /// Mirrors: public void Stop()
        
        public void Stop() => TimerEngineNative.TE_Stop(handle);

        
        /// Check if the timer is currently running
        /// Mirrors: private async Task RunTimerLoopAsync(CancellationToken token)
        
        /// <returns>True if running, false otherwise</returns>
        public bool IsRunning
        {
            get => TimerEngineNative.TE_IsRunning(handle) != 0;
        }
    }
}
