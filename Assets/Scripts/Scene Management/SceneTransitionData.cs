using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.SceneManagement
{
    [CreateAssetMenu(fileName = "SceneTransitionData", menuName = "Lotl/Scene Management/Transition Data")]
    public class SceneTransitionData : ScriptableObject
    {
        [SerializeField] private string referrerScene;
        [SerializeField] private string targetScene;

        public string ReferrerScene => referrerScene;

        public string TargetScene => targetScene;

        public void SetTarget(string referrerScene, string targetScene)
        {
            this.referrerScene = referrerScene;
            this.targetScene = targetScene;
        }
    }
}
