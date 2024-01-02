using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Runtime;
using Lotl.Data.Runs;
using Lotl.Hexgrid;
using Lotl.Units;
using Lotl.Generic.Variables;
using Lotl.Utility;
using Lotl.Generic.Events;

namespace Lotl.Gameplay.Waves
{
    using AutounitSetShorthand = RuntimeSet<AutounitSetAdder>;

    [RequireComponent(typeof(WaveInfoGenerator))]
    public class WaveSummoner : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private RunDataObject crossSceneData;
        [SerializeField] private AutounitRuntimeSet enemyRuntimeSet;
        [SerializeField] private FloatReference hexSize;
        [SerializeField] private IntReference mapSize;
        [SerializeField] private FloatReference subwaveDelay;
        [SerializeField] private GameEvent onWaveEnd;

        [Header("Wave Data")]
        [SerializeField] private List<SquadronType> waveSquadrons;

        #region Internals

        private Transform entryPointParent;
        private List<Transform> entryPoints = new();

        private WaveInfoGenerator waveInfoGenerator;

        private Dictionary<int, PrefabBook> cachedSquadronSollections = new();

        private Coroutine currentWaveCoroutine;
        private bool doneSummoningWave = true;

        #endregion

        #region Initialization Methods
        
        private void Awake()
        {
            waveInfoGenerator = GetComponent<WaveInfoGenerator>();
            InitializeEntryPoints();
        }

        private void InitializeEntryPoints()
        {
            entryPointParent = new GameObject($"Entry Points").transform;
            entryPointParent.parent = transform;

            List<Hex> entryPointDirections = Hex.GetNeighbourVectors();

            int index = 0;
            foreach(Hex direction in entryPointDirections)
            {
                Hex entryPointHex = direction * mapSize;
                Vector3 entryPointPosition = Hex.HexToPixel(entryPointHex, hexSize).xz();

                Transform entryPoint = new GameObject($"Entry Point [{index++}]").transform;
                entryPoint.parent = entryPointParent;
                entryPoint.position = entryPointPosition;
                entryPoint.forward = -entryPoint.position;

                entryPoints.Add(entryPoint);
            }
        }

        #endregion

        private void OnEnable()
        {
            enemyRuntimeSet.Changed += EnemyRuntimeSetChanged;
            WaveEndCheck();
        }

        private void OnDisable()
        {
            enemyRuntimeSet.Changed -= EnemyRuntimeSetChanged;
        }

        #region Wave Methods

        public void StartWave()
        {
            int waveToSummon = crossSceneData.Data.RunInfo.WaveIndex + 1;
            
            WaveInfo currentWave = waveInfoGenerator.GenerateWaveInfo(waveToSummon);
            
            doneSummoningWave = false;
            StartCoroutine(SummonWave(currentWave));
        }

        private IEnumerator SummonWave(WaveInfo waveInfo)
        {
            for (int i = 0; i < waveInfo.SubwaveDifficulties.Count; i++)
            {
                SummonSubwave(waveInfo.EntryPointIndeces, waveInfo.SubwaveDifficulties[i]);
                yield return new WaitForSeconds(subwaveDelay);
            }
            doneSummoningWave = true;
        }

        private void SummonSubwave(List<int> entryPointIndeces, int difficulty)
        {
            for (int i = 0; i < entryPointIndeces.Count; i++)
            {
                Transform entryPoint = entryPoints[entryPointIndeces[i]];

                InstantiateSquadron(entryPoint, difficulty);

                difficulty = waveInfoGenerator.EvaluateSubwaveDifficultyFalloff(
                    i, entryPointIndeces.Count,
                    difficulty);
            }
        }

        private void EnemyRuntimeSetChanged(AutounitSetShorthand enemyRuntimeSet)
        {
            WaveEndCheck();
        }

        private void WaveEndCheck()
        {
            if (doneSummoningWave && enemyRuntimeSet.Items.Count == 0)
            {
                OnWaveEnd();
            }
        }

        private void OnWaveEnd()
        {
            crossSceneData.Data.RunInfo.WaveIndex++;
            onWaveEnd.Raise();
        }

        #endregion

        #region Prefab Methods

        private PrefabBook GetSquadronCollection(int difficulty)
        {
            if (cachedSquadronSollections.ContainsKey(difficulty))
                return cachedSquadronSollections[difficulty];

            SquadronType searchedSquadron = waveSquadrons
                .FirstOrDefault(squadron => squadron.Difficulty == difficulty);

            if (searchedSquadron == null) return null;

            cachedSquadronSollections.Add(difficulty, searchedSquadron.SquadronCollection);
            return cachedSquadronSollections[difficulty];
        }

        private GameObject GetRandomSquadronPrefab(int difficulty)
        {
            PrefabBook squadronCollection = GetSquadronCollection(difficulty);
            
            if(squadronCollection == null) return null;

            int squadronsCount = squadronCollection.Prefabs.Count;
            int randomSquadronIndex = Random.Range(0, squadronsCount);
            GameObject randomSquadron = squadronCollection.Prefabs[randomSquadronIndex];
            return randomSquadron;
        }

        private void InstantiateSquadron(Transform entryPoint, int difficulty)
        {
            GameObject randomSquadronPrefab = GetRandomSquadronPrefab(difficulty);
            
            if(randomSquadronPrefab == null) return;

            GameObject squadron = Instantiate(randomSquadronPrefab,
                entryPoint.position,
                Quaternion.identity,
                entryPoint);

            squadron.transform.forward = entryPoint.forward;
        }

        #endregion
    }

    [System.Serializable]
    internal class SquadronType
    {
        [SerializeField] private int difficulty;
        [SerializeField] private PrefabBook squadronCollection;
        
        public int Difficulty => difficulty;
        public PrefabBook SquadronCollection => squadronCollection;
    }
}