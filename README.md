# UnityCodeSnippet

收集的或者自己写的一些代码片段

## 编辑器功能

### InspectorTool.cs

在Project窗口或者Hierarchy窗口, 右键菜单中增加一项New Inspector. 点击后新创建一个锁定的Inspector窗口. Odin Inspector中有类似功能(Open in new inspector), 但是只能对Inspector面板中的GameObject使用.

* [Question on Unity Answers](https://answers.unity.com/questions/36131/editor-multiple-inspectors.html)

### ProjectSettingsInspector.cs

在Edit - Project Settings的子菜单中增加一项Everything, 点击后创建一个锁定的新窗口, 可以编辑所有的工程设置.

* [Thread on Unity Forum](https://forum.unity.com/threads/new-settings-gui.557308/)
* [Snippet on Bitbucket](https://bitbucket.org/snippets/pschraut/5edXM8/unity-all-project-settings-in-a-single)

### LightmapUvChecker.cs

当一个物件没有UV时, 又被勾选了Lightmap Static, 会报错: (srcData.GetChannelMask() & copyChannels) == copyChannels. 这个工具就是为了修正这个问题.

* [Question on Unity Answers](https://answers.unity.com/questions/1470570/getting-some-error-always-when-i-open-unity.html)

### Animation工具

#### AnimationHierarchyEditor.cs

用来修正Animation中断掉的物件结构, 但同时只能修正1个Animation, 不能应用到Animator下的所有Animation. 功能不如下面这个工具, 但也不是没有用, 可以用于结构已经改变, 要进行修复的情况.

* [Howto: Remapping the Animation Hierarchy in Unity](http://enemyhideout.com/2016/05/howto-remapping-the-animation-hierarchy-in-unity/)

#### Monitor4AnimationCurve

这个工具可以实时监控物件的移动/重命名等操作, 并且立即修正Animation/Animator, 缺点是不能同时修正多个Animation/Animator.

* [Gibhub](https://github.com/gydisme/Unity-Game-Framwork/tree/master/Assets/Editor/CustomEditor/Monitor4AnimationCurve)
* [Question on Unity Answers](https://answers.unity.com/questions/662382/how-to-change-objects-hierarchy-level-in-animation.html)