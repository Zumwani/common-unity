using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Common
{

    public static class TaskUtility
    {

        public static ObjectAwaiter GetAwaiter(this Object obj)                                     => new ObjectAwaiter(obj);
        public static CoroutineAwaiter GetAwaiter(this IEnumerator coroutine)                       => new CoroutineAwaiter(coroutine);
        public static YieldInstructionAwaiter GetAwaiter(this YieldInstruction yieldInstruction)    => new YieldInstructionAwaiter(yieldInstruction);

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

            private bool IsDone { get; set; }

            public CoroutineAwaiter(IEnumerator coroutine) : this()
            {
                var obj = new GameObject("Coroutine Runner");
                obj.AddComponent<MonoBehaviour>().StartCoroutine(Coroutine(coroutine));
            }

            IEnumerator Coroutine(IEnumerator coroutine)
            {
                yield return coroutine;
                IsDone = true;
            }

            public bool IsCompleted => IsDone;

            public void OnCompleted(Action continuation) =>
                continuation?.Invoke();

            public void GetResult() { }

        }

        public struct YieldInstructionAwaiter : INotifyCompletion
        {

            private bool IsDone { get; set; }

            public YieldInstructionAwaiter(YieldInstruction yieldInstruction) : this()
            {
                var obj = new GameObject("Coroutine Runner");
                obj.AddComponent<MonoBehaviour>().StartCoroutine(Coroutine(yieldInstruction));
            }

            IEnumerator Coroutine(YieldInstruction yieldInstruction)
            {
                yield return yieldInstruction;
                IsDone = true;
            }

            public bool IsCompleted => IsDone;

            public void OnCompleted(Action continuation) =>
                continuation?.Invoke();

            public void GetResult() { }

        }

    }

}
