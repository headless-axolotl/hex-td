using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Damage
{
    public class DamageInfo
    {
        [SerializeField] private float amount;
        [SerializeField] private Vector3 source;
        [SerializeField] private DamageTrigger[] triggers;

        public float Amount { get => amount; set => amount = value; }
        public Vector3 Source { get => source; set => source = value; }
        public DamageTrigger[] Triggers { get => triggers; set => triggers = value; }

        public DamageInfo(
            float amount,
            Vector3 source,
            DamageTrigger[] triggers)
        {
            Amount = amount;
            Source = source;
            Triggers = triggers;
        }
    }
}