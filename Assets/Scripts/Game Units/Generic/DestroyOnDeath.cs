using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic
{
    [RequireComponent(typeof(Unit))]
    public class DestroyOnDeath : MonoBehaviour
    {
        private Unit unit;

        private void Awake()
        {
            unit = GetComponent<Unit>();
        }

        private void Start()
        {
            unit.Died += (_) => Destroy(gameObject);
        }
    }
}
