using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Towerset;
using System.IO;

namespace Lotl.Data.Runs
{
    [CreateAssetMenu(fileName = "RunDataManager", menuName = "Lotl/Data/Run Data Manager")]
    public class RunDataObject : ScriptableObject
    {
        [SerializeField] private string runId;
        [SerializeField] private RunInfo runInfo = new();
        [SerializeField] private TowersetInfo towersetInfo = new();

        public string RunId { get => runId; set => runId = value; }
        public RunInfo RunInfo { get => runInfo; set => runInfo = value; }
        public TowersetInfo TowersetInfo { get => towersetInfo; set => towersetInfo = value; }

        public byte[] Serialize()
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);

            byte[] runInfoData = RunInfo.Serialize(RunInfo);
            byte[] towersetInfoData = TowersetInfo.Serialize(towersetInfo);

            writer.Write(runInfoData.Length);
            writer.Write(runInfoData);
            writer.Write(towersetInfoData.Length);
            writer.Write(towersetInfoData);

            return stream.ToArray();
        }

        public void Deserialize(byte[] data, TowerTokenLibrary tokenLibrary)
        {
            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);

            int runInfoDataLength = reader.ReadInt32();
            byte[] runInfoData = reader.ReadBytes(runInfoDataLength);
            int towersetInfoDataLength = reader.ReadInt32();
            byte[] towersetInfoData = reader.ReadBytes(towersetInfoDataLength);

            runInfo = RunInfo.Deserialize(runInfoData);
            towersetInfo = TowersetInfo.Deserialize(towersetInfoData, tokenLibrary);
        }
    }
}
