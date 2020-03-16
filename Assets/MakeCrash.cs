using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Diagnostics;

public class MakeCrash : MonoBehaviour
{
    private void OnGUI()
    {
        var rect = new Rect(50, 50, 300, 50);
        if (GUI.Button(rect, "Crash 1")) MakeCrash1();

        rect.y += 80;

        if (GUI.Button(rect, "Crash 2")) MakeCrash2();

        rect.y += 80;

        if (GUI.Button(rect, "Crash 3")) MakeCrash3();
    }

    [DllImport("kernel32.dll")]
    private static extern void RaiseException(uint dwExceptionCode, uint dwExceptionFlags, uint nNumberOfArguments,
        IntPtr lpArguments);


    private void MakeCrash3()
    {
        RaiseException(13, 0, 0, new IntPtr(1));
    }

    private void MakeCrash2()
    {
        PerformOverflow();
    }

    private void PerformOverflow()
    {
        PerformOverflow();
    }

    private void MakeCrash1()
    {
#if UNITY_2018_3_OR_NEWER
        Utils.ForceCrash(ForcedCrashCategory.AccessViolation);
#endif
    }
}
