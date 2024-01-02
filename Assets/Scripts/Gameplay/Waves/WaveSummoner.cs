using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.AssetManagement;
using Lotl.Runtime;
using Lotl.Data.Runs;

namespace Lotl.Gameplay.Waves
{
    [RequireComponent(typeof(WaveInfoGenerator))]
    public class WaveSummoner : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private RunDataObject crossSceneData;
        [SerializeField] private AutounitRuntimeSet enemyRuntimeSet;

        [Header("Wave Data")]
        [SerializeField] private List<SquadronType> waveSquadrons;

        #region Internals

        private WaveInfoGenerator waveInfoGenerator;

        private Dictionary<int, PrefabBook> cachedSquadronSollections = new();

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
            
        }

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        #endregion

        #region Wave Methods

        public void StartWave()
        {

        }

        IEnumerator SummonWave()
        {
            yield return null;
            doneSummoningWave = false;
        }

        private void TriggerWaveEnd()
        {

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

        private GameObject GetRandomSquadron(int difficulty)
        {
            PrefabBook squadronCollection = GetSquadronCollection(difficulty);
            
            if(squadronCollection == null) return null;

            int squadronsCount = squadronCollection.Prefabs.Count;
            int randomSquadronIndex = Random.Range(0, squadronsCount);
            GameObject randomSquadron = squadronCollection.Prefabs[randomSquadronIndex];
            return randomSquadron;
        }

        private void InstantiateSquadron(int entryPoint, int difficulty)
        {
            GameObject randomSquadron = GetRandomSquadron(difficulty);
            
            if(randomSquadron == null) return;


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