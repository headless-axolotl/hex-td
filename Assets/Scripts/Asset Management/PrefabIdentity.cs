using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.AssetManagement
{
    public class PrefabIdentity : MonoBehaviour
    {
        [SerializeField] private int bookId = -1;
        [SerializeField] private int id = -1;
        
        public int BookId { get => bookId; set => bookId = value; }
        public int Id { get => id; set => id = value; }
    }
}
