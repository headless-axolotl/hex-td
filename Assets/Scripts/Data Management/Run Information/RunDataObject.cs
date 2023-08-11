using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Data.Towerset;

namespace Lotl.Data.Runs
{
    [System.Serializable]
    public class RunData
    {
        [SerializeField] private string runId;
        [SerializeField] private RunInfo runInfo;
        [SerializeField] private TowersetInfo towersetInfo;

        public string RunId
        {
            get => runId;
            set => runId = value;
        }
        public RunInfo RunInfo
        {
            get => runInfo;
            set => runInfo = value;
        }
        public TowersetInfo TowersetInfo
        {
            get => towersetInfo;
            set => towersetInfo = value;
        }

        public static byte[] Serialize(RunData from)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);

            byte[] runInfoData = RunInfo.Serialize(from.RunInfo);
            byte[] towersetInfoData = TowersetInfo.Serialize(from.TowersetInfo);

            writer.Write(runInfoData.Length);
            writer.Write(runInfoData);
            writer.Write(towersetInfoData.Length);
            writer.Write(towersetInfoData);

            return stream.ToArray();
        }

        public static RunData Deserialize(byte[] data, TowerTokenLibrary tokenLibrary)
        {
            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);

            int runInfoDataLength = reader.ReadInt32();
            byte[] runInfoData = reader.ReadBytes(runInfoDataLength);
            int towersetInfoDataLength = reader.ReadInt32();
            byte[] towersetInfoData = reader.ReadBytes(towersetInfoDataLength);

            RunData runData = new();

            runData.RunInfo = RunInfo.Deserialize(runInfoData);
            runData.TowersetInfo = TowersetInfo.Deserialize(towersetInfoData, tokenLibrary);
            
            return runData;
        }
    }


    [CreateAssetMenu(fileName = "RunDataObject", menuName = "Lotl/Data/Run Data Object")]
    public class RunDataObject : ScriptableObject
    {
        [SerializeField] private RunData data;

        public RunData Data
        {
            get => data;
            set => data = value;
        }
    }
}
