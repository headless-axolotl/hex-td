using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Data
{
    public class AsyncTaskHandler : MonoBehaviour
    {
        public void HandleTask(Task task, Action callback)
        {
            StartCoroutine(TaskToCoroutine(task, callback));
        }

        public void HandleTask<T>(Task<T> task, Action<T> callback)
        {
            StartCoroutine(TaskToCoroutine(task, callback));
        }

        private IEnumerator TaskToCoroutine(Task task, Action callback)
        {
            while (!task.IsCompleted) yield return null;
            callback?.Invoke();
        }

        private IEnumerator TaskToCoroutine<T>(Task<T> task, Action<T> callback)
        {
            while (!task.IsCompleted) yield return null;
            callback?.Invoke(task.Result);
        }
    }
}
