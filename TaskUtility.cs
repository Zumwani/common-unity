using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common
{

    public static class TaskUtility
    {

        public static ObjectAwaiter GetAwaiter(this Object obj) => new ObjectAwaiter(obj);
        public static CoroutineAwaiter GetAwaiter(this IEnumerator coroutine) => new CoroutineAwaiter(coroutine);

        public struct ObjectAwaiter : INotifyCompletion
        {

            private readonly Object obj;

            public ObjectAwaiter(Object obj) =>
                this.obj = obj;

            public bool IsCompleted => obj;

            public void OnCompleted(Action continuation) =>
                continuation?.Invoke();

            public void GetResult() { }

        }

        public struct CoroutineAwaiter : INotifyCompletion
        {

            static readonly List<IEnumerator> completed = new List<IEnumerator>();
            readonly IEnumerator coroutine;

            public CoroutineAwaiter(IEnumerator coroutine) : this()
            {

                this.coroutine = coroutine;
                Run().StartCoroutine();

                IEnumerator Run()
                {
                    yield return coroutine;
                    completed.Add(coroutine);
                    continuation?.Invoke();
                }

            }

            public bool IsCompleted
            {
                get
                {
                    if (completed.Contains(coroutine))
                    {
                        completed.Remove(coroutine);
                        return true;
                    }
                    return false;
                }
            }

            Action action;

            public void OnCompleted(Action continuation)
            {
                action = continuation;
            }

            public void GetResult() { }

        }

    }

}
