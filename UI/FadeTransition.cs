using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Common.Transitions
{

    //TODO: Overexposure animation when faded in for game over screen

    public static class FadeTransition
    {

        public static IEnumerator Fade(this CanvasGroup group, float to, float duration)
        {

            if (!group.gameObject.activeInHierarchy)
                yield break;

            if (to == 0)
                group.interactable = false;

            if (group.alpha == to)
                yield break;

            yield return LerpUtility.Lerp(group.alpha, to, duration, t => group.alpha = t, OnComplete);

            void OnComplete()
            {
                if (to == 1)
                    group.interactable = true;
            }

        }

        /// <summary>Fades the screen.</summary>
        /// <param name="fadeOut">Return false to delay fade out, useful for scene transitions.</param>
        /// <param name="color">The color of the fade, defaults to black.</param>
        public static void Fade(this Camera _, float duration, Color? color = null, Func<bool> fadeOut = null)
        {

            if (!Application.isPlaying)
                return;

            (CanvasGroup group, Image image) = ScreenTransitionHelper.CreateTopmostImage();

            image.StartCoroutine(DoFade());

            IEnumerator DoFade()
            {

                image.color = color ?? Color.black;

                group.alpha = 0;
                yield return group.Fade(1, duration);

                if (fadeOut != null)
                    while (!fadeOut.Invoke())
                        yield return null;

                yield return group.Fade(0, duration);

                if (image && image.canvas)
                    image.canvas.gameObject.Destroy();

            }

        }

    }

}
