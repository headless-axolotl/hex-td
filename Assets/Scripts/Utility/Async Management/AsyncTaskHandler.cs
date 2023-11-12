using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Utility.Async
{
    using Result = AsyncTaskResult;

    public class AsyncTaskHandler : MonoBehaviour
    {
        public void HandleTask(Task task, Action<Result> onComplete, Action onSuccess)
        {
            StartCoroutine(TaskToCoroutine(task, onComplete, onSuccess));
        }

        public void HandleTask<TResult>(
            Task<TResult> task,
            Action<Result> onComplete,
            Action<TResult> onSuccess)
        {
            StartCoroutine(TaskToCoroutine(task, onComplete, onSuccess));
        }

        public void HandleTask<TResult, TOutput>(
            Task<TResult> task,
            Action<Result, TOutput> onComplete,
            Func<TResult, TOutput> onSuccess)
        {
            StartCoroutine(TaskToCoroutine(task, onComplete, onSuccess));
        }

        private IEnumerator TaskToCoroutine(
            Task task,
            Action<Result> onComplete,
            Action onSuccess)
        {
            while (!task.IsCompleted) yield return null;

            if (!task.IsFaulted)
            {
                onSuccess?.Invoke();
                onComplete?.Invoke(Result.OK);
            }
            else onComplete?.Invoke(new(true, task.Exception?.Message));
        }

        private IEnumerator TaskToCoroutine<TResult>(
            Task<TResult> task,
            Action<Result> onComplete,
            Action<TResult> onSuccess)
        {
            while (!task.IsCompleted) yield return null;

            if (!task.IsFaulted)
            {
                onSuccess?.Invoke(task.Result);
                onComplete?.Invoke(Result.OK);
            }
            else onComplete?.Invoke(new(true, task.Exception?.Message));
        }

        private IEnumerator TaskToCoroutine<TResult, TOutput>(
            Task<TResult> task,
            Action<Result, TOutput> onComplete,
            Func<TResult, TOutput> onSuccess)
        {
            while (!task.IsCompleted) yield return null;
            
            if (!task.IsFaulted && onSuccess != null)
            {
                TOutput output = onSuccess(task.Result);
                onComplete?.Invoke(Result.OK, output);
            }
            else onComplete?.Invoke(new(true, task.Exception?.Message), default);
        }
    }
}
