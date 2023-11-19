using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Lotl.Generic.Variables;

namespace Lotl.SceneManagement
{
    public class SceneTransitionInitializer : MonoBehaviour
    {
        [SerializeField] private StringReference transitionScene;
        [SerializeField] private SceneTransitionData sceneTransitionData;

        private void Awake()
        {
            if(sceneTransitionData == null)
            {
                Debug.LogError("Missing scene transition data!");
            }
        }

        public void StartSceneTransition(StringVariable targetScene)
        {
            StartSceneTransition(targetScene.Value);
        }

        public void StartSceneTransition(string targetScene)
        {
            string referrerScene = SceneManager.GetActiveScene().name;
            sceneTransitionData.SetTarget(referrerScene, targetScene);
            SceneManager.LoadScene(transitionScene, LoadSceneMode.Single);
        }

        public void ReturnToPreviousScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            sceneTransitionData.SetTarget(currentScene, sceneTransitionData.ReferrerScene);
            SceneManager.LoadScene(transitionScene, LoadSceneMode.Single);
        }
    }
}
