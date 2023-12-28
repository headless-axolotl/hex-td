using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;
using Lotl.Units.Towers;

namespace Lotl.Data.Runs
{
    [System.Serializable]
    public class RunInfo
    {
        [SerializeField] private int resources = 0;
        [SerializeField] private int waveIndex = 1;
        [SerializeField] private List<TowerInfo> towersData;

        public int Resources { get => resources; set => resources = value; }
        public int WaveIndex { get => waveIndex; set => waveIndex = value; }
        public IReadOnlyList<TowerInfo> TowersData => towersData;

        public RunInfo()
        {
            towersData = new();
        }

        public RunInfo(IEnumerable<TowerInfo> towersData)
        {
            this.towersData = towersData.ToList();
        }

        public static void ExtractTowerInfo(RunInfo into, AutounitRuntimeSet towerRuntimeSet)
        {
            into.towersData.Clear();
            foreach (AutounitSetAdder tower in towerRuntimeSet.Items)
            {
                if (TowerInfo.TryExtractInfo(tower.gameObject, out var towerInfo))
                    into.towersData.Add(towerInfo);
            }
        }

        public static byte[] Serialize(RunInfo runInfo)
        {
            using MemoryStream memoryStream = new();
            using BinaryWriter writer = new(memoryStream);

            writer.Write(runInfo.Resources);
            writer.Write(runInfo.WaveIndex);

            writer.Write(runInfo.TowersData.Count);
            foreach (TowerInfo towerInfo in runInfo.TowersData)
            {
                writer.Write(towerInfo.CurrentHealth);
                writer.Write(towerInfo.BookIndex);
                writer.Write(towerInfo.PrefabIndex);
                writer.Write(towerInfo.Position.q);
                writer.Write(towerInfo.Position.r);
            }

            return memoryStream.ToArray();
        }

        public static RunInfo Deserialize(byte[] data)
        {
            RunInfo runInfo = new();
            using MemoryStream memoryStream = new(data);
            using BinaryReader reader = new(memoryStream);

            runInfo.Resources = reader.ReadInt32();
            runInfo.WaveIndex = reader.ReadInt32();

            int towerCount = reader.ReadInt32();
            for (int i = 0; i < towerCount; i++)
            {
                runInfo.towersData.Add(new()
                {
                    CurrentHealth = reader.ReadSingle(),
                    BookIndex = reader.ReadInt32(),
                    PrefabIndex = reader.ReadInt32(),
                    Position = new(reader.ReadInt32(), reader.ReadInt32()),
                });
            }

            return runInfo;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.Append($"Resources: {Resources}\n");
            stringBuilder.Append($"Wave Index: {WaveIndex}\n");
            foreach (TowerInfo towerInfo in towersData)
                stringBuilder.Append($"{towerInfo}\n");
            return stringBuilder.ToString();
        }
    }
}
