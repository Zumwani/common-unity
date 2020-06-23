using UnityEngine;

namespace Common
{

    public static class VectorUtility
    {

        public static Vector2 With(this Vector2 v, float? x = null, float? y = null)                        => new Vector2((x ?? v.x), (y ?? v.y));
        public static Vector3 With(this Vector3 v, float? x = null, float? y = null, float? z = null)       => new Vector3((x ?? v.x), (y ?? v.y), (z ?? v.z));

        public static Vector2 Add(this Vector2 v, float? x = null, float? y = null)                         => new Vector2(v.x + (x ?? 0), v.y + (y ?? 0));
        public static Vector3 Add(this Vector3 v, float? x = null, float? y = null, float? z = null)        => new Vector3(v.x + (x ?? 0), v.y + (y ?? 0), v.z + (z ?? 0));

        public static Vector2 Subtract(this Vector2 v, float? x = null, float? y = null)                    => new Vector2(v.x - (x ?? 0), v.y - (y ?? 0));
        public static Vector3 Subtract(this Vector3 v, float? x = null, float? y = null, float? z = null)   => new Vector3(v.x - (x ?? 0), v.y - (y ?? 0), v.z - (z ?? 0));

        public static Vector2 Multiply(this Vector2 v, float? x = null, float? y = null)                    => new Vector2(v.x * (x ?? 0), v.y * (y ?? 0));
        public static Vector3 Multiply(this Vector3 v, float? x = null, float? y = null, float? z = null)   => new Vector3(v.x * (x ?? 0), v.y * (y ?? 0), v.z * (z ?? 0));

        public static Vector2 Divide(this Vector2 v, float? x = null, float? y = null)                      => new Vector2(v.x / (x ?? 0), v.y / (y ?? 0));
        public static Vector3 Divide(this Vector3 v, float? x = null, float? y = null, float? z = null)     => new Vector3(v.x / (x ?? 0), v.y / (y ?? 0), v.z / (z ?? 0));

    }

}
