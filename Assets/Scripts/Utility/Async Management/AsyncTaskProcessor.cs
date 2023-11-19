using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Utility.Async
{
    using Result = AsyncTaskResult;

    public class AsyncTaskProcessor : MonoBehaviour
    {
        /// <summary>
        /// <br>Handles a task which does not return an object.</br>
        /// <br>If the task is completed successfully <paramref name="onSuccess"/> is called.</br>
        /// <br>In every case afterwards <paramref name="onComplete"/> is called with the corresponding <see cref="Result"/>.</br>
        /// </summary>
        /// <param name="task">The task which will be processed.</param>
        /// <param name="onComplete">A callback called when the task is completed.</param>
        /// <param name="onSuccess">A callback called when the task is successfully completed.</param>
        public void ProcessTask(Task task, Action<Result> onComplete, Action onSuccess)
        {
            StartCoroutine(TaskToCoroutine(task, onComplete, onSuccess));
        }

        /// <summary>
        /// <br>Handles a task which returns an object (which <paramref name="onComplete"/> does not consume).</br>
        /// <br>If the task is completed successfully <paramref name="onSuccess"/> is called consuming the output of the task.</br>
        /// <br>In every case afterwards <paramref name="onComplete"/> is called with the corresponding <see cref="Result"/>.</br>
        /// </summary>
        /// <param name="task">The task which will be processed.</param>
        /// <param name="onComplete">A callback when the task is completed.</param>
        /// <param name="onSuccess">A callback called when the task is successfully completed.</param>
        public void ProcessTask<TResult>(
            Task<TResult> task,
            Action<Result> onComplete,
            Action<TResult> onSuccess)
        {
            StartCoroutine(TaskToCoroutine(task, onComplete, onSuccess));
        }

        /// <summary>
        /// <br>Handles a task which returns an object.</br>
        /// <br>If the task is completed successfully <paramref name="onSuccess"/>
        /// is called transforming the output from the task.</br>
        /// <br>In every case afterwards  <paramref name="onComplete"/> is called with the corresponding</br>
        /// <br><see cref="Result"/> and output from <paramref name="onSuccess"/> (or default(<typeparamref name="TOutput"/>) if the task failed).</br>
        /// </summary>
        /// <param name="task">The task which will be processed.</param>
        /// <param name="onComplete">A callback when the task is completed.</param>
        /// <param name="onSuccess">A func which transforms the output from the task.</param>
        public void ProcessTask<TResult, TOutput>(
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
            else onComplete?.Invoke(new(false, task.Exception?.Message));
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
            else onComplete?.Invoke(new(false, task.Exception?.Message));
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
            else onComplete?.Invoke(new(false, task.Exception?.Message), default);
        }
    }
}
