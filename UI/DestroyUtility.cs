using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common
{

    public static class DestroyUtility
    {

        /// <summary>Uses <see cref="Object.Destroy(Object)"/> while game is running, <see cref="Object.DestroyImmediate(Object)"/> otherwise.</summary>
        public static void Destroy(this Object obj)
        {
            if (obj)
                if (Application.isPlaying)
                    Object.Destroy(obj);
                else
                    Object.DestroyImmediate(obj);
        }

    }

}