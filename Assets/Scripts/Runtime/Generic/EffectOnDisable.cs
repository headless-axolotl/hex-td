using Lotl.Generic.Variables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime.Generic
{
    public class EffectOnDisable : MonoBehaviour
    {
        [SerializeField] private Pool effectPool;
        [SerializeField] private FloatReference disableAfter;
        private GameObject currentEffect = null;

        private void DisableCurrentEffect()
        {
            if (currentEffect == null) return;
            currentEffect.SetActive(false);
            currentEffect = null;
        }

        private void OnDisable()
        {
            currentEffect = effectPool.GetObject();
            currentEffect.transform.position = transform.position;
            currentEffect.SetActive(true);
            Invoke(nameof(DisableCurrentEffect), disableAfter);
        }
    }
}