using System;
using System.Collections;
using UnityEngine;

namespace Common
{
    public static class LerpUtility
    {

        public static IEnumerator Lerp(float start, float end, float duration, Action<float> callback, Action onComplete = null)
        {

            var t = 0f;
            var time = 0f;

            while (t <= 1)
            {

                callback?.Invoke(Mathf.Lerp(start, end, t));

                time += Time.deltaTime;
                t = time / duration;
                yield return null;

            }

            callback?.Invoke(end);
            onComplete?.Invoke();

        }

    }

}
