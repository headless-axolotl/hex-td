using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Runtime.Generic
{
    public class Timer : MonoBehaviour
    {
        public event Action Done;
        
        [SerializeField] private FloatReference defaultDuration;
        private bool isTicking;
        private Coroutine currentCoroutine;

        public bool IsTicking => isTicking;

        private void Awake()
        {
            isTicking = false;
        }

        public void Trigger(float duration = 0f)
        {
            if (isTicking) return;
            isTicking = true;
            currentCoroutine = StartCoroutine(
                Tick(duration == 0f ? defaultDuration : duration));
        }

        public void Stop()
        {
            isTicking = false;
            if (currentCoroutine != null)
                StopCoroutine(currentCoroutine);
        }

        private IEnumerator Tick(float duration)
        {
            yield return new WaitForSeconds(duration);
            isTicking = false;
            currentCoroutine = null;
            Done?.Invoke();
        }
    }

}
