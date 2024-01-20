using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units;
using Lotl.Generic.Variables;
using Lotl.Generic.Events;

namespace Lotl.Gameplay.UnitBehaviours
{
    [RequireComponent(typeof(Unit))]
    public class RaiseEventOnDeath : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private GameEvent gameEvent;

        private Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            unit.Died += RaiseEvent;
        }

        void RaiseEvent(Unit unit)
        {
            gameEvent.Raise();
            unit.Died -= RaiseEvent;
        }
    }
}