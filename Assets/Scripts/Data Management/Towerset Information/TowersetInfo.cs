using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Lotl.Data.Towerset
{
    public class TowersetInfo
    {
        private List<TowerToken> towerTokens;

        public IReadOnlyList<TowerToken> TowerTokens => towerTokens;

        public TowersetInfo()
        {
            towerTokens = new();
        }

        public static TowersetInfo Deserialize(byte[] data, TowerTokenLibrary library)
        {
            TowersetInfo towersetInfo = new();

            foreach (byte b in data)
            {
                towersetInfo.towerTokens.Add(library.GetToken(b));
            }

            towersetInfo.towerTokens = towersetInfo.towerTokens
                .OrderBy(token => token.IndexInLibrary).ToList();

            return towersetInfo;
        }

        public static byte[] Serialize(TowersetInfo towersetInfo)
        {
            using MemoryStream stream = new();
            using BinaryWriter writer = new(stream);
            foreach(TowerToken token in towersetInfo.towerTokens)
            {
                writer.Write((byte)token.IndexInLibrary);
            }
            return stream.ToArray();
        }
    }
}
