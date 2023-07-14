using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.AssetManagement
{
    public class PrefabIdentity : MonoBehaviour
    {
        [SerializeField] private int bookId;
        [SerializeField] private int id;
        
        public int BookId { get => bookId; set => bookId = value; }
        public int Id { get => id; set => id = value; }
    }
}
