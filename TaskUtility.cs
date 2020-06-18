using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
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

            public async void OnCompleted(Action continuation)
            {
                while (!IsCompleted)
                    await Task.Yield();
                continuation?.Invoke();
            }

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

            }

            IEnumerator Run()
            {
                yield return coroutine;
                completed.Add(coroutine);
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

            public async void OnCompleted(Action continuation)
            {
                while (!completed.Contains(coroutine))
                    await Task.Yield();
                continuation?.Invoke();
            }

            public void GetResult() { }

        }

    }

}
