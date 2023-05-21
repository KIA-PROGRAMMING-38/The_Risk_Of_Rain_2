using System;

using UnityEngine;


// Ease와 관련된 유틸리티 함수들 입니다.
// https://easings.net/ko
// https://gist.github.com/cjddmut/d789b9eb78216998e95c​

public static class LerpUtil
    {


        public static float Linear(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            return Mathf.Lerp(start, end, value);
        }​

        public static float Spring(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value = (Mathf.Sin(value * Mathf.PI * (0.2f + 2.5f * value * value * value)) * Mathf.Pow(1f - value, 2.2f) + value) * (1f + (1.2f * (1f - value)));
            return start + (end - start) * value;
        }

        public static Vector2 Spring(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = Spring(start.x, end.x, value);
            result.y = Spring(start.y, end.y, value);
            return result;
        }​

        public static float EaseInQuad(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * value * value + start;
        }

        public static Vector2 EaseInQuad(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInQuad(start.x, end.x, value);
            result.y = EaseInQuad(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutQuad(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return -end * value * (value - 2) + start;
        }
        public static Vector2 EaseOutQuad(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutQuad(start.x, end.x, value);
            result.y = EaseOutQuad(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutQuad(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value + start;
            value--;
            return -end * 0.5f * (value * (value - 2) - 1) + start;
        }
        public static Vector2 EaseInOutQuad(Vector2 start, Vector2 end, float value)
        {
            value = Mathf.Clamp01(value);
            Vector2 result = new Vector2();
            result.x = EaseInOutQuad(start.x, end.x, value);
            result.y = EaseInOutQuad(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInCubic(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * value * value * value + start;
        }
        public static Vector2 EaseInCubic(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInCubic(start.x, end.x, value);
            result.y = EaseInCubic(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutCubic(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value--;
            end -= start;
            return end * (value * value * value + 1) + start;
        }
        public static Vector2 EaseOutCubic(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutCubic(start.x, end.x, value);
            result.y = EaseOutCubic(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutCubic(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value + 2) + start;
        }
        public static Vector2 EaseInOutCubic(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutCubic(start.x, end.x, value);
            result.y = EaseInOutCubic(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInQuart(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * value * value * value * value + start;
        }
        public static Vector2 EaseInQuart(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutCubic(start.x, end.x, value);
            result.y = EaseInOutCubic(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutQuart(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value--;
            end -= start;
            return -end * (value * value * value * value - 1) + start;
        }
        public static Vector2 EaseOutQuart(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutQuart(start.x, end.x, value);
            result.y = EaseOutQuart(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutQuart(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value + start;
            value -= 2;
            return -end * 0.5f * (value * value * value * value - 2) + start;
        }
        public static Vector2 EaseInOutQuart(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutQuart(start.x, end.x, value);
            result.y = EaseInOutQuart(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInQuint(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * value * value * value * value * value + start;
        }
        public static Vector2 EaseInQuint(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInQuint(start.x, end.x, value);
            result.y = EaseInQuint(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutQuint(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value--;
            end -= start;
            return end * (value * value * value * value * value + 1) + start;
        }
        public static Vector2 EaseOutQuint(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutQuint(start.x, end.x, value);
            result.y = EaseOutQuint(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutQuint(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * value * value * value * value * value + start;
            value -= 2;
            return end * 0.5f * (value * value * value * value * value + 2) + start;
        }
        public static Vector2 EaseInOutQuint(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutQuint(start.x, end.x, value);
            result.y = EaseInOutQuint(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInSine(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return -end * Mathf.Cos(value * (Mathf.PI * 0.5f)) + end + start;
        }
        public static Vector2 EaseInSine(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInSine(start.x, end.x, value);
            result.y = EaseInSine(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutSine(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * Mathf.Sin(value * (Mathf.PI * 0.5f)) + start;
        }
        public static Vector2 EaseOutSine(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutSine(start.x, end.x, value);
            result.y = EaseOutSine(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutSine(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return -end * 0.5f * (Mathf.Cos(Mathf.PI * value) - 1) + start;
        }
        public static Vector2 EaseInOutSine(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutSine(start.x, end.x, value);
            result.y = EaseInOutSine(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInExpo(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * Mathf.Pow(2, 10 * (value - 1)) + start;
        }
        public static Vector2 EaseInExpo(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInExpo(start.x, end.x, value);
            result.y = EaseInExpo(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutExpo(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return end * (-Mathf.Pow(2, -10 * value) + 1) + start;
        }
        public static Vector2 EaseOutExpo(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutExpo(start.x, end.x, value);
            result.y = EaseOutExpo(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInOutExpo(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value /= .5f;
            end -= start;
            if (value < 1) return end * 0.5f * Mathf.Pow(2, 10 * (value - 1)) + start;
            value--;
            return end * 0.5f * (-Mathf.Pow(2, -10 * value) + 2) + start;
        }
        public static Vector2 EaseInOutExpo(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInOutExpo(start.x, end.x, value);
            result.y = EaseInOutExpo(start.y, end.y, value);
            return result;
        }
​
        public static float EaseInCirc(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            end -= start;
            return -end * (Mathf.Sqrt(1 - value * value) - 1) + start;
        }
        public static Vector2 EaseInCirc(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseInCirc(start.x, end.x, value);
            result.y = EaseInCirc(start.y, end.y, value);
            return result;
        }
​
        public static float EaseOutCirc(float start, float end, float value)
        {
            value = Mathf.Clamp01(value);
            value--;
            end -= start;
            return end * Mathf.Sqrt(1 - value * value) + start;
        }
        public static Vector2 EaseOutCirc(Vector2 start, Vector2 end, float value)
        {
            Vector2 result = new Vector2();
            result.x = EaseOutCirc(start.x, end.x, value);
            result.y = EaseOutCirc(start.y, end.y, value);
            return result;
        }​
}

