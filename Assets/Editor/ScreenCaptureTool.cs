using System;
using UnityEditor;
using UnityEngine;

public static class ScreenCaptureTool
{
    [MenuItem("Tools/截屏")]
    static void CaptureScreenshot()
    {
        string filename = string.Format("screenshot_{0:yyyyMMddhhmmss}.png", DateTime.Now);
        ScreenCapture.CaptureScreenshot(filename);
    }
}
