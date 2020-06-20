using UnityEngine;

namespace Common
{
    public static class RectUtility
    {

        public static Rect With(this Rect r, float? x = null, float? y = null, float? width = null, float? height = null) =>
            new Rect((x ?? r.x), (y ?? r.y), (width ?? r.width), (height ?? r.height));

        public static Rect Expand(this Rect r, float? x = null, float? y = null, float? width = null, float? height = null) =>
            new Rect((r.x - (x ?? 0)), (r.y - (y ?? 0)), (r.width + (width ?? 0)), (r.height + (height ?? 0)));

        public static Rect Shrink(this Rect r, float? x = null, float? y = null, float? width = null, float? height = null) =>
            new Rect((r.x + (x ?? 0)), (r.y + (y ?? 0)), (r.width - (width ?? 0)), (r.height - (height ?? 0)));

    }

}
