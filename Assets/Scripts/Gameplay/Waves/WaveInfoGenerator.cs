using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Generic.Variables;

namespace Lotl.Gameplay.Waves
{
    public class WaveInfoGenerator : MonoBehaviour
    {
        private readonly int[] EntryPointIndeces = { 0, 1, 2, 3, 4, 5 };
        
        [Header("Wave Data")]
        [SerializeField] private IntReference maxWaveCount;
        [SerializeField] private IntReference minEntryPointCount;
        [SerializeField] private IntReference maxSubwaveCount;
        [SerializeField] private IntReference maxWaveDifficulty;
        [SerializeField] private AnimationCurveReference currentWaveEntryPointCountFunction;
        [SerializeField] private AnimationCurveReference currentWaveSubwaveCountFunction;
        [SerializeField] private AnimationCurveReference currentWaveMaxDifficultyFunction;
        [SerializeField] private AnimationCurveReference currentSubwaveDifficultyFunction;

        public int DefaultMaxWaveCount => maxWaveCount;

        public WaveInfo GenerateWaveInfo(int currentWave, int maxWave)
        {
            int waveEntryPointCount = EvaluateWaveEntryPointCount(currentWave, maxWave);
            List<int> currentEntryPointIndeces = EntryPointIndeces
                .OrderBy(item => Random.Range(0f, 1f))
                .Take(waveEntryPointCount).ToList();

            int subwaveCount = EvaluateWaveSubwaveCount(currentWave, maxWave);
            int waveMaxDifficulty = EvaluateWaveMaxDifficulty(currentWave, maxWave);

            List<int> subwaveDifficulties = new(subwaveCount);
            for (int i = 1; i <= subwaveCount; i++)
            {
                int currentSubwaveDifficulty = EvaluateSubwaveDifficulty(i, subwaveCount, waveMaxDifficulty);
                subwaveDifficulties.Add(currentSubwaveDifficulty);
            }

            WaveInfo waveInfo = new()
            {
                EntryPointIndeces = currentEntryPointIndeces,
                SubwaveDifficulties = subwaveDifficulties
            };

            return waveInfo;
        }

        private int EvaluateWaveEntryPointCount(
            float currentWave,
            float maxWave)
        {
            float time = currentWave / maxWave;
            if (time > 1) time = 1;
            float value = currentWaveEntryPointCountFunction.Value.Evaluate(time) * EntryPointIndeces.Length;
            value = Mathf.Max(minEntryPointCount, value);
            return Mathf.CeilToInt(value);
        }

        private int EvaluateWaveSubwaveCount(
            float currentWave,
            float maxWave)
        {
            float time = currentWave / maxWave;
            if (time > 1) time = 1;
            float value = currentWaveSubwaveCountFunction.Value.Evaluate(time) * maxSubwaveCount;
            return Mathf.CeilToInt(value);
        }

        private int EvaluateWaveMaxDifficulty(
            float currentWave,
            float maxWave)
        {
            float time = currentWave / maxWave;
            if (time > 1) time = 1;
            float value = currentWaveMaxDifficultyFunction.Value.Evaluate(time) * maxWaveDifficulty;
            return Mathf.CeilToInt(value);
        }

        private int EvaluateSubwaveDifficulty(
            float currentSubwave,
            float maxSubwave,
            float waveMaxDifficulty)
        {
            float time = currentSubwave / maxSubwave;
            if (time > 1) time = 1;
            float value = currentSubwaveDifficultyFunction.Value.Evaluate(time) * waveMaxDifficulty;
            return Mathf.CeilToInt(value);
        }
    }

    public class WaveInfo
    {
        public List<int> EntryPointIndeces { get; set; }
        public List<int> SubwaveDifficulties { get; set; }
    }
}
