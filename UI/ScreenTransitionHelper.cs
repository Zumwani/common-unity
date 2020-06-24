using UnityEngine;
using UnityEngine.UI;

namespace Common.Transitions
{

    static class ScreenTransitionHelper
    {

        public static (CanvasGroup group, Image image) CreateTopmostImage()
        {

            var canvas = new GameObject("Screen Transition Helper").AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;

            Object.DontDestroyOnLoad(canvas.gameObject);

            var image = new GameObject("Image").AddComponent<Image>();
            image.gameObject.transform.SetParent(canvas.transform);
            image.rectTransform.anchorMin = Vector2.zero;
            image.rectTransform.anchorMax = Vector2.one;
            image.rectTransform.anchoredPosition = Vector3.zero;

            image.gameObject.AddComponent<GraphicRaycaster>();

            var group = image.gameObject.AddComponent<CanvasGroup>();

            return (group, image);

        }

    }

}
