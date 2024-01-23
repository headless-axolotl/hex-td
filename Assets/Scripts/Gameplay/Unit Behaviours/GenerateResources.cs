using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Gameplay.UnitBehaviours
{
    public class GenerateResources : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private IntReference resourcePerTick;
        [SerializeField] private FloatReference tickRate;
        [SerializeField] private IntVariable resources;

        private float tickDelay = 0f;
        private Coroutine generationCoroutine;
        
        private void Awake()
        {
            tickDelay = 1 / (tickRate + float.Epsilon);
        }

        public void StartGeneration()
        {
            if (generationCoroutine != null)
                StopGeneration();
            generationCoroutine = StartCoroutine(DoGeneration());
        }

        public void StopGeneration()
        {
            if(generationCoroutine != null)
            {
                StopCoroutine(generationCoroutine);
            }
        }

        private IEnumerator DoGeneration()
        {
            while (true)
            {
                resources.Value += resourcePerTick;
                yield return new WaitForSeconds(tickDelay);
            }
        }
    }
}