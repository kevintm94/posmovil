﻿using Android.App;
using Android.Runtime;
using System;
using Shiny;

namespace POSMovil.Droid
{
    #if DEBUG
        [Application(Debuggable = true)]
    #else
            [Application(Debuggable = false)]
    #endif


    public class MainApplication : ShinyAndroidApplication<ShinyAppStartup>
    {
            public MainApplication(IntPtr handle, JniHandleOwnership transfer) : base(handle, transfer)
            {
            }
        
    }
}