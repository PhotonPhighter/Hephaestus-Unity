﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for Logging-to-file functions
public class Logger : ILoopable
{
    public static Logger MainLog = new Logger();
    private readonly List<string> mainlogtxt = new List<string>();

    public static void Instantiate() => MainLoopable.GetInstance().RegisterLoops(MainLog);

    public static void Log(string ll) => MainLog.log(ll);
    public static void Log(System.Exception e) => MainLog.log(e);
    public void log(string ll) => this.mainlogtxt.Add(ll);
    public void log(System.Exception e) => this.mainlogtxt.Add(e.StackTrace.ToString());

    public void Start() { }

    public void Update() => System.IO.File.WriteAllLines("Log.txt", new List<string>(this.mainlogtxt).ToArray());

    public void OnApplicationQuit()
    {
        
    }
}