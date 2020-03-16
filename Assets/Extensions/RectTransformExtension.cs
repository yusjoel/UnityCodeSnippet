using UnityEngine;

namespace Extensions
{
    // http://orbcreation.com/orbcreation/page.orb?1099

    public static class RectTransformExtension
    {
        public static void SetDefaultScale(this RectTransform trans)
        {
            trans.localScale = new Vector3(1, 1, 1);
        }

        public static void SetPivotAndAnchors(this RectTransform trans, Vector2 aVec)
        {
            trans.pivot = aVec;
            trans.anchorMin = aVec;
            trans.anchorMax = aVec;
        }

        public static Vector2 GetSize(this RectTransform trans)
        {
            return trans.rect.size;
        }

        public static float GetWidth(this RectTransform trans)
        {
            return trans.rect.width;
        }

        public static float GetHeight(this RectTransform trans)
        {
            return trans.rect.height;
        }

        public static void SetPositionOfPivot(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x, newPos.y, trans.localPosition.z);
        }

        public static void SetLeftBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + trans.pivot.x * trans.rect.width, newPos.y + trans.pivot.y * trans.rect.height,
                trans.localPosition.z);
        }

        public static void SetLeftTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x + trans.pivot.x * trans.rect.width,
                newPos.y - (1f - trans.pivot.y) * trans.rect.height, trans.localPosition.z);
        }

        public static void SetRightBottomPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - (1f - trans.pivot.x) * trans.rect.width,
                newPos.y + trans.pivot.y * trans.rect.height, trans.localPosition.z);
        }

        public static void SetRightTopPosition(this RectTransform trans, Vector2 newPos)
        {
            trans.localPosition = new Vector3(newPos.x - (1f - trans.pivot.x) * trans.rect.width,
                newPos.y - (1f - trans.pivot.y) * trans.rect.height, trans.localPosition.z);
        }

        public static void SetSize(this RectTransform trans, Vector2 newSize)
        {
            var oldSize = trans.rect.size;
            var deltaSize = newSize - oldSize;
            trans.offsetMin = trans.offsetMin - new Vector2(deltaSize.x * trans.pivot.x, deltaSize.y * trans.pivot.y);
            trans.offsetMax = trans.offsetMax + new Vector2(deltaSize.x * (1f - trans.pivot.x), deltaSize.y * (1f - trans.pivot.y));
        }

        public static void SetWidth(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(newSize, trans.rect.size.y));
        }

        public static void SetHeight(this RectTransform trans, float newSize)
        {
            SetSize(trans, new Vector2(trans.rect.size.x, newSize));
        }

        // http://answers.unity.com/comments/1461672/view.html
        public static void SetPivot(this RectTransform rectTransform, Vector2 pivot)
        {
            if (rectTransform == null) return;

            var scale = rectTransform.localScale;
            var size = rectTransform.rect.size;
            var deltaPivot = rectTransform.pivot - pivot;
            var deltaPosition = new Vector3(
                deltaPivot.x * size.x * scale.x,
                deltaPivot.y * size.y * scale.y);
            //deltaPosition = rectTransform.rotation * deltaPosition;
            deltaPosition = Quaternion.Euler(rectTransform.localEulerAngles) * deltaPosition;
            rectTransform.pivot = pivot;
            rectTransform.localPosition -= deltaPosition;
        }

        // http://answers.unity.com/answers/1100504/view.html

        /// <summary>
        /// Converts RectTransform.rect's local coordinates to world space
        /// Usage example RectTransformExt.GetWorldRect(myRect, Vector2.one);
        /// </summary>
        /// <returns>The world rect.</returns>
        /// <param name="rt">RectangleTransform we want to convert to world coordinates.</param>
        /// <param name="scale">Optional scale pulled from the CanvasScaler. Default to using Vector2.one.</param>
        public static Rect GetWorldRect(this RectTransform rt, Vector2 scale)
        {
            // Convert the rectangle to world corners and grab the top left
            Vector3[] corners = new Vector3[4];
            rt.GetWorldCorners(corners);
            Vector3 topLeft = corners[0];

            // Rescale the size appropriately based on the current Canvas scale
            Vector2 scaledSize = new Vector2(scale.x * rt.rect.size.x, scale.y * rt.rect.size.y);

            return new Rect(topLeft, scaledSize);
        }
    }
}
