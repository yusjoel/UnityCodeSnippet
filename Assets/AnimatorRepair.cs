using System.Collections.Generic;
using UnityEngine;

/// <summary>
///     动画修正
/// </summary>
public class AnimatorRepair : MonoBehaviour
{
    private readonly List<RectInfo> rectInfos = new List<RectInfo>();

    private void Awake()
    {
        var rectTransforms = GetComponentsInChildren<RectTransform>(true);
        foreach (var rectTransform in rectTransforms)
            rectInfos.Add(new RectInfo(rectTransform));
    }

    private void OnDisable()
    {
        foreach (var rectInfo in rectInfos)
            rectInfo.Repair();
    }
    // Animator在激活的时候会保存各个物件的初始值, 从而保证每个Animation在播放的时候是从这个初始值开始的.
    // 如果动画修改的都是同一样的东西, 如位移, 那么是看不出区别的. 但如果动画分别修改两样东西, 如一组修改位移, 另一组修改缩放.
    // 然后在缩放到最小的时候关闭了物件, 再开启物件, Animator就会重新初始化, 重新保存物件的初始值.
    // 此时播放缩放这一组动画没有问题, 但是播放位移那一组动画, 就会发现物件都是在缩小状态.

    // https://forum.unity.com/threads/reset-animator-usage-with-pooled-object.290792/
    // #16 Ash-blue 给出了一个方案就是先保存所有值, 再写回去

    private class RectInfo
    {
        private readonly Vector2 anchoredPosition;

        private readonly Vector3 localScale;

        private readonly RectTransform rectTransform;

        public RectInfo(RectTransform rectTransform)
        {
            this.rectTransform = rectTransform;
            localScale = rectTransform.localScale;
            anchoredPosition = rectTransform.anchoredPosition;
        }

        public void Repair()
        {
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.localScale = localScale;
        }
    }
}
