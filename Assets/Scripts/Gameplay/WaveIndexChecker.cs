using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Data.Runs;
using Lotl.Generic.Variables;
using UnityEngine.Events;

namespace Lotl.Gameplay
{
    public class WaveIndexChecker : MonoBehaviour
    {
        [SerializeField] private IntReference waveIndex;
        [SerializeField] private RunDataObject runData;
        [SerializeField] private UnityEvent indexReached;

        public void CheckWaveIndex()
        {
            if (runData.Data.RunInfo.WaveIndex == waveIndex)
                indexReached.Invoke();
        }
    }
}