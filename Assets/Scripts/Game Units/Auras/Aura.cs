using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units.Aura
{
    public abstract class Aura : MonoBehaviour
    {
        public event Action OnAuraTick;

        [Header("Data")]
        [SerializeField] private FloatReference auraRadius;
        [SerializeField] private FloatReference effectRate;
        [SerializeField] private UnitTribeMask scanTribeMask;
        [SerializeField] private LayerMask scanMask;

        private float tickDelay = 1f;
        private bool paused = false;

        private void Awake()
        {
            tickDelay = 1 / (effectRate + float.Epsilon);
        }

        private void Start()
        {
            Unpause();
            StartCoroutine(DoAura());
        }

        private void OnEnable()
        {
            Unpause();
        }

        private void OnDisable()
        {
            Pause();
        }

        public void Pause() => paused = true;
        public void Unpause() => paused = false;

        protected virtual IEnumerator DoAura()
        {
            while (true)
            {
                if(!paused)
                {
                    List<Unit> validUnits = GetValidUnits();
                    AuraTick(validUnits);
                    yield return new WaitForSeconds(tickDelay);
                }
                else yield return null;
            }
        }

        protected virtual List<Unit> GetValidUnits()
        {
            return Scanner.Scan(transform.position, auraRadius, scanMask, scanTribeMask);
        }

        protected abstract void AuraTick(List<Unit> units);

        public void InvokeTickEvent() => OnAuraTick?.Invoke();

#if UNITY_EDITOR

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, auraRadius);
        }

#endif
    }
}