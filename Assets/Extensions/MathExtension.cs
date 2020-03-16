using UnityEngine;

namespace Extensions
{
    public static class MathExtension
    {
        /// <summary>
        ///     将一个角度限制在最小值和最大值的范围内, 角度最终取值会向最靠近的上下限靠拢
        /// </summary>
        /// <param name="angle">任意角度</param>
        /// <param name="min">-180 ~ 180</param>
        /// <param name="max">-180 ~ 180</param>
        /// <returns>-180 ~ 180</returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            // 将角度转成0~360
            angle = Mathf.Repeat(angle, 360);
            // 转成-180~180
            if (angle > 180f)
                angle -= 360f;

            // min = -180, max = 0的情况下, angle从-180偏移到-181
            // angle会变成179, 直接使用Mathf.Clamp()得到的结果会是0, 而实际期望得到的是-180
            // 但如果说偏移是+359, 那么希望得到0, 这就...

            if (angle >= min && angle <= max)
                return angle;

            float deltaMin = Mathf.Abs(Mathf.DeltaAngle(angle, min));
            float deltaMax = Mathf.Abs(Mathf.DeltaAngle(angle, max));
            return deltaMin > deltaMax ? max : min;
        }
    }
}
