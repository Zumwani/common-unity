using UnityEngine;

namespace Common
{

    public static class LayerMaskUtility
    {

        public static bool Compare(this LayerMask mask, int layer) =>
            (((1 << layer) & mask) != 0);

    }

}
