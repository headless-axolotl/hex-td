using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Lotl.StateMachine;

namespace Lotl.Units.Attackers
{
    public class StaticSummoner : Driver
    {
        public event Action OnSummonAction;

        [Header("Data")]
        [SerializeField] private FloatReference summonRate;
        [SerializeField] private FloatReference summonDelay;
        [SerializeField] private IntReference summonCap;
        [SerializeField] private List<Transform> summonPoints;
        [Tooltip("Should have a Unit component.")]
        [SerializeField] private GameObject unitPrefab;

        private HashSet<Unit> summonedUnits = new();
        private float summonCooldown;
        private int summonPointIndex = 0;

        protected override void Awake()
        {
            summonCooldown = 1 / summonRate + float.Epsilon;

            base.Awake();
        }

        protected override void Start()
        {
            StartCoroutine(DoSummoning());

            base.Start();
        }

        private IEnumerator DoSummoning()
        {
            while (true)
            {
                if (ShouldSummon())
                {
                    InvokeSummonAction();
                    yield return new WaitForSeconds(summonDelay);
                    Summon();
                    yield return new WaitForSeconds(summonCooldown);
                }
                else yield return null;
            }
        }

        protected virtual bool ShouldSummon()
        {
            return !IsPaused && summonedUnits.Count < summonCap;
        }

        private Transform GetNextSummonPoint()
        {
            Transform summonPoint = summonPoints[summonPointIndex];
            summonPointIndex++;
            if (summonPointIndex >= summonPoints.Count) summonPointIndex = 0;
            return summonPoint;
        }

        protected virtual void Summon()
        {
            Transform summonPoint = GetNextSummonPoint();

            GameObject unitGameObject = Instantiate(
                unitPrefab,
                summonPoint.position,
                summonPoint.rotation,
                summonPoint);

            if(!unitGameObject.TryGetComponent(out Unit unit))
            {
                Debug.LogWarning("UnitPrefab is missing a Unit component!");
                return;
            }

            summonedUnits.Add(unit);
            unit.Died += RemoveSummonedUnitOnDeath;
        }

        private void RemoveSummonedUnitOnDeath(Unit unit)
        {
            summonedUnits.Remove(unit);
            unit.Died -= RemoveSummonedUnitOnDeath;
        }

        public void InvokeSummonAction()
        {
            OnSummonAction?.Invoke();
        }

        private void OnValidate()
        {
            if (unitPrefab.GetComponent<Unit>() == null)
            {
                Debug.LogWarning("Prefab does not have Unit component.");
                unitPrefab = null;
            }
        }
    }
}