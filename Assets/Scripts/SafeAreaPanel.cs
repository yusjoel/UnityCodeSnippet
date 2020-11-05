using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    /// <summary>
    ///     对于异形屏的安全区域的适配
    /// </summary>
    public class SafeAreaPanel : MonoBehaviour
    {
        /// <summary>
        ///     是否需要修正CanvasScaler
        /// </summary>
        public bool NeedFixCanvasScaler;

        public bool IsDebugMode;

        private RectTransform rectTransform;

        private CanvasScaler canvasScaler;

        private Canvas canvas;
        private float canvasScaleFactor;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasScaler = GetComponentInParent<CanvasScaler>();
            canvas = canvasScaler.GetComponent<Canvas>();

            if (IsDebugMode)
            {
                Debug.LogFormat("Screen width: {0}, height:{1}, resolution: {2}", Screen.width, Screen.height, Screen.currentResolution);
                var mainCamera = Camera.main;
                if (mainCamera != null)
                    Debug.LogFormat("Camera viewport: width {0}, height {1}", mainCamera.pixelWidth, mainCamera.pixelHeight);
            }

#if UNITY_EDITOR
            // 编辑器下对iPhoneX进行模拟
            var safeArea = new Rect(132f, 63f, 2172f, 1062f);
            if (Screen.width == 2436 && Screen.height == 1125)
                Simulate(safeArea);
#else
            var safeArea = Screen.safeArea;
            if (NeedFixCanvasScaler)
                FixCanvasScaler(safeArea);
            ApplySafeArea(safeArea, Screen.width, Screen.height);
#endif
        }

        /// <summary>
        ///     如果CanvasScaler是Scale With Screen Size, 需要修正Canvas.scaleFactor
        /// </summary>
        private void FixCanvasScaler(Rect safeArea)
        {
            if (!canvasScaler || !canvas) return;

            if (canvasScaler.uiScaleMode != CanvasScaler.ScaleMode.ScaleWithScreenSize)
                return;

            // https://bitbucket.org/Unity-Technologies/ui/src/0651862509331da4e85f519de88c99d0529493a5/UnityEngine.UI/UI/Core/Layout/CanvasScaler.cs?at=2018.3%2Fstaging

            var referenceResolution = canvasScaler.referenceResolution;
            canvasScaleFactor = 0;
            switch (canvasScaler.screenMatchMode)
            {
                case CanvasScaler.ScreenMatchMode.MatchWidthOrHeight:
                    {
                        const int logBase = 2;
                        float logWidth = Mathf.Log(safeArea.width / referenceResolution.x, logBase);
                        float logHeight = Mathf.Log(safeArea.height / referenceResolution.y, logBase);
                        float logWeightedAverage = Mathf.Lerp(logWidth, logHeight, canvasScaler.matchWidthOrHeight);
                        canvasScaleFactor = Mathf.Pow(logBase, logWeightedAverage);
                        break;
                    }
                case CanvasScaler.ScreenMatchMode.Expand:
                    {
                        canvasScaleFactor = Mathf.Min(safeArea.width / referenceResolution.x, safeArea.height / referenceResolution.y);
                        break;
                    }
                case CanvasScaler.ScreenMatchMode.Shrink:
                    {
                        canvasScaleFactor = Mathf.Max(safeArea.width / referenceResolution.x, safeArea.height / referenceResolution.y);
                        break;
                    }
            }



            // 参考: https://forum.unity.com/threads/understanding-canvas-scaler-screen-match-mode-and-reference-resolution.524194/
        }

        private void Update()
        {
            if (canvas && canvasScaleFactor > 0)
                canvas.scaleFactor = canvasScaleFactor;
        }

        private void HandleMatchWidthOrHeight(Rect safeArea)
        {
            // sx, sy是屏幕的宽高, rx, ry是参考分辨率, nx, ny是异形屏的宽高
            // scale = (sx / rx) ^ (1 - match) * (sy / ry) ^ match
            // 可见match = 0时, 缩放为sx/rx即按照宽度缩放, match = 1时, 缩放为sy/ry即按照高度缩放
            // 由于CanvasScale使用的是屏幕的宽高而不是安全区域的宽高, 所以需要调整match使得
            // (nx / rx) ^ (1 - match) * (ny / ry) ^ match = (sx / rx) ^ (1 - match') * (sy / ry) ^ match'
            // 整理得到:
            // match' = lg(nx / sx * ((ny / ry) * (rx / nx)) ^ match) / lg((sy / ry) * (rx / sx))
            float sx = Screen.width;
            float sy = Screen.height;
            var referenceResolution = canvasScaler.referenceResolution;
            float rx = referenceResolution.x;
            float ry = referenceResolution.y;
            float nx = safeArea.width;
            float ny = safeArea.height;
            float match = canvasScaler.matchWidthOrHeight;
            // 真数
            float nature = nx / sx * Mathf.Pow(ny / ry * rx / nx, match);
            // 底数
            float bottom = sy / ry * rx / sx;
            // 用换底公式计算对数
            float logarithm = Mathf.Log(nature) / Mathf.Log(bottom);
            canvasScaler.matchWidthOrHeight = logarithm;
        }

        private void ApplySafeArea(Rect area, int screenWidth, int screenHeight)
        {
            var anchorMin = area.position;
            var anchorMax = area.position + area.size;
            anchorMin.x /= screenWidth;
            anchorMin.y /= screenHeight;
            anchorMax.x /= screenWidth;
            anchorMax.y /= screenHeight;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
        }

        private void Simulate(Rect safeArea)
        {
            if (IsDebugMode)
                Debug.LogFormat("[Simulate iPhone X]Screen.width {0}, height {1}, safeArea.position {2}, size {3}, Screen.safeArea {4}",
                    Screen.width, Screen.height, safeArea.position, safeArea.size, Screen.safeArea);

            if (NeedFixCanvasScaler)
                FixCanvasScaler(safeArea);

            ApplySafeArea(safeArea, Screen.width, Screen.height);
        }
    }
}
