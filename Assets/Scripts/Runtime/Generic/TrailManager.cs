using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime.Generic
{
    [RequireComponent(typeof(TrailRenderer))]
    public class TrailManager : MonoBehaviour
    {
        private TrailRenderer trailRenderer;
        
        private void Awake()
        {
            trailRenderer = GetComponent<TrailRenderer>();
        }

        private void OnEnable()
        {
            trailRenderer.Clear();
        }
    }
}