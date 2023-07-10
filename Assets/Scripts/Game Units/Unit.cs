using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Units
{
    public class Unit : MonoBehaviour
    {
        #region Events

        public event EventHandler WasDamaged;
        public event EventHandler WasHealed;
        public event EventHandler Died;

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

        public virtual void TakeDamage(float amount)
        {
            if (amount <= 0) return;

            health = Mathf.Max(health - amount, 0);
            WasDamaged?.Invoke(this, null);
            
            if (health <= 0) Died?.Invoke(this, null);
        }

        public virtual void Heal(float amount)
        {
            if (amount <= 0) return;

            health = Mathf.Min(health + amount, maxHealth);
            WasHealed?.Invoke(this, null);
        }

        #endregion
    }
}

