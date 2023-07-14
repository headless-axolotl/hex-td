using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.DataManagement
{
    [CreateAssetMenu(fileName = "RunDataManager", menuName = "Lotl/Data/Run Data Manager")]
    public class RunDataManager : ScriptableObject
    {
        [SerializeField] private string runId;
        [SerializeField] private RunInfo runInfo = new();

        public string RunId { get => runId; set => runId = value; }
        public RunInfo RunInfo { get => runInfo; set => runInfo = value; }

        public void DeserializeRunInfo(byte[] data)
        {
            using MemoryStream memoryStream = new(data);
            using BinaryReader reader = new(memoryStream);
            
            runInfo.Resources = reader.ReadInt32();
            
            runInfo.TowersData.Clear();
            int towerCount = reader.ReadInt32();
            for (int i = 0; i < towerCount; i++)
            {
                runInfo.TowersData.Add(new()
                {
                    CurrentHealth = reader.ReadSingle(),
                    BookIndex = reader.ReadInt32(),
                    PrefabIndex = reader.ReadInt32(),
                    Position = new(reader.ReadInt32(), reader.ReadInt32()),
                });
            }
        }

        public byte[] SerializeRunInfo()
        {
            using MemoryStream memoryStream = new();
            using BinaryWriter writer = new(memoryStream);

            writer.Write(runInfo.Resources);
            
            writer.Write(runInfo.TowersData.Count);
            foreach(TowerInfo towerInfo in runInfo.TowersData)
            {
                writer.Write(towerInfo.CurrentHealth);
                writer.Write(towerInfo.BookIndex);
                writer.Write(towerInfo.PrefabIndex);
                writer.Write(towerInfo.Position.q);
                writer.Write(towerInfo.Position.r);
            }

            return memoryStream.ToArray();
        }
    }
}
