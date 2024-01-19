using Lotl.Data.Towerset;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Lotl.Data.Users
{
    [System.Serializable]
    public class UserData
    {
        [SerializeField] private int metaCurrencyAmount;
        [SerializeField] private TowersetInfo unlockedTowers;

        public int MetaCurrencyAmount
        {
            get => metaCurrencyAmount;
            set => metaCurrencyAmount = value;
        }

        public TowersetInfo UnlockedTowers
        {
            get => unlockedTowers;
            set => unlockedTowers = value;
        }

        public UserData(int metaCurrencyAmount, TowersetInfo unlockedTowers)
        {
            this.metaCurrencyAmount = metaCurrencyAmount;
            this.unlockedTowers = unlockedTowers;
        }

        public static byte[] Serialize(UserData userData)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);

            writer.Write(userData.MetaCurrencyAmount);

            byte[] towersetInfoData = TowersetInfo.Serialize(userData.UnlockedTowers);
            writer.Write(towersetInfoData.Length);
            writer.Write(towersetInfoData);

            return stream.ToArray();
        }

        public static UserData Deserialize(byte[] data, TowerTokenLibrary library)
        {
            using MemoryStream stream = new(data);
            using BinaryReader reader = new(stream);

            int metaCurrencyAmount = reader.ReadInt32();
            
            int towersetInfoDataLength = reader.ReadInt32();
            byte[] towersetInfoData = reader.ReadBytes(towersetInfoDataLength);
            TowersetInfo towersetInfo = TowersetInfo.Deserialize(towersetInfoData, library);

            UserData userData = new(metaCurrencyAmount, towersetInfo);

            return userData;
        }
    }
}