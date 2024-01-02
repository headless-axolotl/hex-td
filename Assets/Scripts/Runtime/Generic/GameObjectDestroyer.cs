using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Runtime.Generic
{
    public class GameObjectDestroyer : MonoBehaviour
    {
        [SerializeField] private GameObject target;
        public void DestroyTarget() => Destroy(target);
    }
}
