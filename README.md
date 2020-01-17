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

### CleanUpMaterials.cs

材质在修改了着色器之后还是会保留原来的属性, 所以也会保留对原纹理的引用, 这个脚本用于清理这些不用的数学.

* [Thread on Unity Forum](https://forum.unity.com/threads/material-asset-keeps-references-to-assets-that-are-not-used.523192/)

### Animation工具

#### AnimationHierarchyEditor.cs

用来修正Animation中断掉的物件结构, 但同时只能修正1个Animation, 不能应用到Animator下的所有Animation. 功能不如下面这个工具, 但也不是没有用, 可以用于结构已经改变, 要进行修复的情况.

* [Howto: Remapping the Animation Hierarchy in Unity](http://enemyhideout.com/2016/05/howto-remapping-the-animation-hierarchy-in-unity/)

#### Monitor4AnimationCurve

这个工具可以实时监控物件的移动/重命名等操作, 并且立即修正Animation/Animator, 缺点是不能同时修正多个Animation/Animator.

* [Github](https://github.com/gydisme/Unity-Game-Framwork/tree/master/Assets/Editor/CustomEditor/Monitor4AnimationCurve)
* [Question on Unity Answers](https://answers.unity.com/questions/662382/how-to-change-objects-hierarchy-level-in-animation.html)

## 非编辑器功能

### AnimatorRepair.cs

Animator在激活的时候会保存各个物件的初始值, 从而保证每个Animation在播放的时候是从这个初始值开始的.
如果动画修改的都是同一样的东西, 如位移, 那么是看不出区别的. 但如果动画分别修改两样东西, 如一组修改位移, 另一组修改缩放.
然后在缩放到最小的时候关闭了物件, 再开启物件, Animator就会重新初始化, 重新保存物件的初始值.
此时播放缩放这一组动画没有问题, 但是播放位移那一组动画, 就会发现物件都是在缩小状态.
当然最正确的方式应该是对每一个要修改的属性都赋初值
这个脚本仅仅处理UI上的偏移和缩放

* [Thread on Unity Forum](https://forum.unity.com/threads/reset-animator-usage-with-pooled-object.290792/) #16 Ash-blue
