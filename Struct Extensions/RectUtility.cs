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

        /// <summary>Takes a region from the specified sides.</summary>
        public static Rect Take(this Rect r, float? left, float? top, float? right, float? bottom)
        {

            Rect r2 = new Rect();

            if (left.HasValue) r2.xMax = r.xMin + left.Value;
            if (right.HasValue) r2.xMin = r.xMin + r.width - right.Value;
            
            if (top.HasValue) r2.xMin = r.yMin + r.height - top.Value;
            if (bottom.HasValue) r2.xMax = r.yMin + bottom.Value;

            return r2;

        }

    }

}
