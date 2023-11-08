using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Lotl.Utility
{
    public class AsyncTaskHandler : MonoBehaviour
    {
        public void HandleTask(Task task, Action<bool> callback)
        {
            StartCoroutine(TaskToCoroutine(task, callback));
        }

        public void HandleTask<T>(Task<T> task, Action<T, bool> callback)
        {
            StartCoroutine(TaskToCoroutine(task, callback));
        }

        private IEnumerator TaskToCoroutine(Task task, Action<bool> callback)
        {
            while (!task.IsCompleted) yield return null;

            if (!task.IsFaulted)
            {
                callback?.Invoke(true);
            }
            else callback?.Invoke(false);
        }

        private IEnumerator TaskToCoroutine<T>(Task<T> task, Action<T, bool> callback)
        {
            while (!task.IsCompleted) yield return null;
            
            if (!task.IsFaulted)
            {
                callback?.Invoke(task.Result, true);
            }
            else callback?.Invoke(default, false);
        }
    }
}
