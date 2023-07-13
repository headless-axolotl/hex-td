using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.DataManagement
{
    public class RunDataManager : ScriptableObject
    {
        [SerializeField] private string runId;
        [SerializeField] private RunInfo currentSnapshot;

        public string RunId { get => runId; set => runId = value; }
        public RunInfo CurrentSnapshot { get => currentSnapshot; set => currentSnapshot = value; }

        public void DeserializeRunInfo(byte[] data)
        {
            using MemoryStream memoryStream = new(data);
            using BinaryReader reader = new(memoryStream);
            
            currentSnapshot.ResourceAmount = reader.ReadInt32();
            
            currentSnapshot.TowersData.Clear();
            int towerCount = reader.ReadInt32();
            for (int i = 0; i < towerCount; i++)
            {
                currentSnapshot.TowersData.Add(new()
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

            writer.Write(currentSnapshot.ResourceAmount);
            
            writer.Write(currentSnapshot.TowersData.Count);
            foreach(TowerInfo towerInfo in currentSnapshot.TowersData)
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
