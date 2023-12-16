using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.SceneManagement
{
    public class CrossSceneScriptableObjectLoader : MonoBehaviour
    {
        [SerializeField] private List<ScriptableObject> scriptableObjects = new();
    }
}
