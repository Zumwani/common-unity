using UnityEngine;

namespace Common
{

    public static class ColorUtility
    {

        public static Color With(this Color v, float? r = null, float? g = null, float? b = null, float? a = null) => new Color((r ?? v.r), (g ?? v.g), (b ?? v.b), (a ?? v.a));
        public static Color Add(this Color v, float? r = null, float? g = null, float? b = null, float? a = null) => new Color(v.r + (r ?? 0), v.g + (g ?? 0), v.b + (b ?? 0), v.a + (a ?? 0));
        public static Color Subtract(this Color v, float? r = null, float? g = null, float? b = null, float? a = null) => new Color(v.r - (r ?? 0), v.g - (g ?? 0), v.b - (b ?? 0), v.a - (a ?? 0));
        public static Color Multiply(this Color v, float? r = null, float? g = null, float? b = null, float? a = null) => new Color(v.r * (r ?? 0), v.g * (g ?? 0), v.b * (b ?? 0), v.a * (a ?? 0));
        public static Color Divide(this Color v, float? r = null, float? g = null, float? b = null, float? a = null) => new Color(v.r / (r ?? 0), v.g / (g ?? 0), v.b / (b ?? 0), v.a / (a ?? 0));

    }

}
