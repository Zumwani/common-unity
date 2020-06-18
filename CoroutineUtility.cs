using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Common
{

    public static class CoroutineUtility
    {

        static readonly Dictionary<Coroutine, CoroutineHelper> coroutines = new Dictionary<Coroutine, CoroutineHelper>();

        public static Coroutine StartCoroutine(this IEnumerator coroutine)
        {

            var obj = new GameObject("Coroutine Runner");
            var runner = obj.AddComponent<CoroutineHelper>();
            Coroutine c = null;
            c = runner.StartCoroutine(Run());

            return c;

            IEnumerator Run()
            {
                yield return null;
                coroutines.Add(c, runner);
                yield return coroutine;
                coroutines.Remove(c);
                Object.Destroy(obj);
            }

        }

        public static void CancelCoroutine(Coroutine coroutine)
        {
            if (coroutines.TryGetValue(coroutine, out var runner))
                Object.Destroy(runner);
        }

        public class CoroutineHelper : MonoBehaviour
        { }

    }

}
