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
