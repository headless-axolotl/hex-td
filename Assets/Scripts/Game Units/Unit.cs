using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;
using Lotl.Units.Damage;

namespace Lotl.Units
{
    public class Unit : MonoBehaviour
    {
        #region Events

        public event Action<DamageInfo> WasDamaged;
        public event Action<float> WasHealed;
        public event Action<float> HealthChanged;
        public event Action<Unit> Died;

        #endregion

        #region Properties

        [SerializeField] private FloatReference maxHealth;
        [SerializeField] private float health;
        [SerializeField] private UnitTribe tribe;

        public float MaxHealth => maxHealth;
        public float Health => health;
        public UnitTribe Tribe {
            get
            {
                if (tribe == null)
                    Debug.LogError($"Unit [{name}] does not have a tribe!");
                return tribe;
            }
        }

        #endregion

        #region Methods

        private void Awake()
        {
            health = maxHealth;
        }

        public virtual void TakeDamage(float amount, Vector3 source = new(), params DamageTrigger[] damageTriggers)
        {
            if (amount <= 0) return;

            health = Mathf.Max(health - amount, 0);
            WasDamaged?.Invoke(new(amount, source, damageTriggers));
            HealthChanged?.Invoke(health);
            
            if (health <= 0) Died?.Invoke(this);
        }

        public virtual void Heal(float amount)
        {
            if (amount <= 0) return;

            health = Mathf.Min(health + amount, maxHealth);
            WasHealed?.Invoke(amount);
            HealthChanged?.Invoke(health);
        }

        public void SetCurrentHealth(float value)
        {
            health = Mathf.Clamp(value, 0, maxHealth);
            HealthChanged?.Invoke(health);
        }

        #endregion
    }
}

