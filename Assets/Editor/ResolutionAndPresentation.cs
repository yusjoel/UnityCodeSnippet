using UnityEditor;
using UnityEngine;

public class ResolutionAndPresentation
{
    public void Modify()
    {
        // Resolution and Presentation
        //  Resolution

        // FullScreenWindow, 分辨率可以程序设置也可以在初始的对话框中选择, 但最后会缩放到显示器的分辨率
        // ExclusiveFullscreen, 修改显示器的分辨率, 仅在Windows下有效, 其他平台会变成FullScreenWindow
        // Maximized Window, 最大化窗口, 仅在MacOS有效, 其他平台会变成FullScreenWindow
        // Windowed, 窗口模式, 如果选择该模式, resizableWindow起作用, 并且默认为true
        PlayerSettings.fullScreenMode = FullScreenMode.FullScreenWindow;

        // 使用默认分辨率
        PlayerSettings.defaultIsNativeResolution = true;

        // 支持Mac的Retina屏
        PlayerSettings.macRetinaSupport = true;

        // 失去焦点时仍然运行, 反之则是暂停 (如用于下载资源, 挂机等)
        PlayerSettings.runInBackground = true;

        //  Standalone Player Options

        // 一个显示器全屏不会将另一个显示器变暗
        PlayerSettings.captureSingleScreen = true;

        // Disabled, 永不显示
        // Enabled, 默认显示, 可以在对话框中关闭, 按Alt或者Shift运行则显示
        // HiddenByDefault, 默认不显示, 按Alt或者Shift运行则显示, 也可以在运行时加入参数-show-screen-selector
        // 2019不再支持分辨率对话框
        //PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.HiddenByDefault;

        // 记录日志, 可以加入参数-logfile指定日志存放路径, 否则存放在C:\Users\UserName\AppData\LocalLow\CompanyName\AppName
        PlayerSettings.usePlayerLog = true;

        // 窗口模式下, 窗口是否可以改变大小
        PlayerSettings.resizableWindow = true;

        // 全屏模式下, 失去焦点仍然可见 (仅Windows)
        PlayerSettings.visibleInBackground = true;

        // 运行系统的全屏键切换全屏和窗口模式
        PlayerSettings.allowFullscreenSwitch = true;

        // 单例运行
        PlayerSettings.forceSingleInstance = true;

        // 支持的分辨率
        PlayerSettings.HasAspectRatio(AspectRatio.Aspect16by9);
        PlayerSettings.SetAspectRatio(AspectRatio.Aspect16by9, true);
    }
}
