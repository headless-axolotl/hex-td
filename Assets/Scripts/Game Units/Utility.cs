using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lotl.Units
{
    public static class Utility
    {
        public static List<TComponent> Scan<TComponent>(
            Vector3 position, float radius,
            LayerMask layerMask, int maxElements = 64)
            where TComponent : Component
        {
            Collider[] colliders = new Collider[maxElements];
            Physics.OverlapSphereNonAlloc(position, radius, colliders, layerMask);
            List<TComponent> filtered = new();
            foreach (Collider collider in colliders)
            {
                if (collider.TryGetComponent<TComponent>(out var component))
                    filtered.Add(component);
            }
            return filtered;
        }

        public static List<Unit> Scan(
            Vector3 position, float radius,
            LayerMask layerMask, UnitTribeMask tribeMask)
        {
            return Scan<Unit>(position, radius, layerMask)
                .Where(u => tribeMask.Contains(u.Tribe))
                .ToList();
        }
    }
}
