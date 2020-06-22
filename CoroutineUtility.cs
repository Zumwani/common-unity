#pragma warning disable IDE0062 // Make local function 'static': Unity does not support this...
#pragma warning disable IDE0051 // Remove unused private members

using UnityEngine;
using System.Collections;
using System;
using Object = UnityEngine.Object;
using System.Collections.Generic;
using UnityEditor;
using System.Threading.Tasks;

namespace Common
{

    public static class CoroutineUtility
    {

        static CoroutineRoot root;
        /// <summary>Runs the coroutine. This method can start any number of instances of this coroutine.</summary>
        public static Coroutine StartCoroutine(this IEnumerator coroutine, Action onComplete = null)
        {

            if (coroutine == null)
                return null;

            if (!root)
                root = new GameObject("Coroutine Runner").AddComponent<CoroutineRoot>();

            var obj = new GameObject(coroutine.ToString().Replace("+<", ".").Replace(">", "()").Remove(coroutine.ToString().Length - 4, 4));
            obj.transform.SetParent(root.transform);
            var runner = obj.AddComponent<CoroutineHelper>();
            var c = new Coroutine(coroutine, runner, onComplete);
            runner.Run(coroutine, c);

            return c;

        }
           
        [UnityEditor.Callbacks.DidReloadScripts]
        static void OnScriptsReloaded()
        {
            var obj = Object.FindObjectsOfType<CoroutineRoot>();
            foreach (var o in obj)
                Object.DestroyImmediate(o.gameObject);
        }

        [ExecuteAlways]
        class CoroutineRoot : MonoBehaviour
        {

#if UNITY_EDITOR

            private void Start()
            {
                if (!Application.isPlaying)
                    PushFramesInEditor();
            }

            void PushFramesInEditor()
            {
                EditorApplication.update -= EditorApplication.QueuePlayerLoopUpdate;
                EditorApplication.update += EditorApplication.QueuePlayerLoopUpdate;
                EditorApplication.QueuePlayerLoopUpdate();
            }

#endif

            public void DestroyIfEmpty() =>
                StartCoroutine(WaitAndDestroy(gameObject));
                
            public void Destroy(CoroutineHelper coroutine)
            {
                StartCoroutine(WaitAndDestroy(coroutine.gameObject));
                StartCoroutine(WaitAndDestroy(gameObject, 2));
            }
               
            IEnumerator WaitAndDestroy(GameObject obj, int frames = 1)
            {
                  
                for (int i = 0; i < frames; i++)
                    yield return null;

                if (obj == gameObject && transform.childCount != 0)
                    yield break;

                if (Application.isPlaying)
                    Destroy(obj);
                else
                    DestroyImmediate(obj);

            }

        }

        static readonly Dictionary<string, Coroutine> coroutines = new Dictionary<string, Coroutine>();
        /// <summary>Starts this coroutine. This method can only start a single instance of this coroutine.</summary>
        public static void Start(this IEnumerator coroutine)
        {
            if (coroutine == null)
                return;
            if (!coroutines.ContainsKey(coroutine.ToString()))
                coroutines.Add(coroutine.ToString(), StartCoroutine(coroutine));
        }

        /// <summary>Restarts this coroutine.</summary>
        public static void Restart(this IEnumerator coroutine)
        {
            coroutine.Stop();
            coroutine.Start();
        }

        /// <summary>Stops this coroutine.</summary>
        public static void Stop(this IEnumerator coroutine) => coroutine?.GetActive()?.Stop();

        /// <summary>Pauses this coroutine.</summary>
        public static void Pause(this IEnumerator coroutine) => coroutine?.GetActive()?.Pause();

        /// <summary>Resumes this coroutine, if paused.</summary>
        public static void Resume(this IEnumerator coroutine) => coroutine?.GetActive()?.Resume();

        /// <summary>Gets if this coroutine is currently running. Use IsPaused to check if the coroutine is paused.</summary>
        public static bool IsRunning(this IEnumerator coroutine) => coroutine?.GetActive()?.IsRunning ?? false;

        /// <summary>Gets if this coroutine is currently paused.</summary>
        public static bool IsPaused(this IEnumerator coroutine) => coroutine?.GetActive() is Coroutine c && c.IsPaused && c.IsRunning;

        /// <summary>Gets the currently active instance that was started from Start.</summary>
        public static Coroutine GetActive(this IEnumerator coroutine)
        {
            if (coroutines.TryGetValue(coroutine?.ToString(), out var c))
                return c;
            else
                return null;
        }

        public class Coroutine
        {

            public Coroutine(IEnumerator coroutine, CoroutineHelper helper, Action onComplete)
            {
                this.helper = helper;
                this.coroutine = coroutine;
                OnComplete = onComplete;
            }

            readonly IEnumerator coroutine;
            readonly CoroutineHelper helper;

            public Action OnComplete { get; private set; }
            public bool IsPaused { get; private set; }
            public bool IsRunning => helper;

            public void Pause() => IsPaused = true;
            public void Resume() => IsPaused = false;
            public void Stop()
            {
                OnComplete?.Invoke();
                coroutines.Remove(coroutine.ToString());
                root.DestroyIfEmpty();
                if (Application.isPlaying)
                    Object.Destroy(helper.gameObject);
                else
                    Object.DestroyImmediate(helper.gameObject);
            }

        }
          
        [ExecuteAlways]
        public class CoroutineHelper : MonoBehaviour
        {
                
            public void Run(IEnumerator coroutine, Coroutine helper)
            {
                 
                StartCoroutine(RunCoroutine(coroutine));

                IEnumerator RunCoroutine(IEnumerator c)
                {

                    yield return RunSub(c);

                    IEnumerator RunSub(IEnumerator sub)
                    {
                        while (sub.MoveNext())
                        {

                            while (helper.IsPaused)
                                yield return null;

                            if (sub.Current is IEnumerator sub1)
                                yield return RunSub(sub1);

                            yield return null;

                        }
                    }

                    helper.Stop();

                }

            }

        }

    }

}
