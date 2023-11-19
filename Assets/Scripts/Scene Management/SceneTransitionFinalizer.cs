using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Lotl.SceneManagement
{
    public class SceneTransitionFinalizer : MonoBehaviour
    {
        [SerializeField] private SceneTransitionData sceneTransitionData;
        [SerializeField] private Slider loadingBar;

        private void Awake()
        {
            if (sceneTransitionData == null)
            {
                Debug.LogError("Missing scene transition data!");
                return;
            }

            if(loadingBar == null)
            {
                Debug.LogError("Missing loading bar!");
            }

            StartCoroutine(FinalizeTransition());
        }

        IEnumerator FinalizeTransition()
        {
            AsyncOperation transitionOperation = SceneManager.LoadSceneAsync(sceneTransitionData.TargetScene, LoadSceneMode.Single);
            
            while (!transitionOperation.isDone)
            {
                float remappedProgress = transitionOperation.progress / DoneLoadingProgress;
                loadingBar.value = remappedProgress;
                yield return null;
            }
        }

        const float DoneLoadingProgress = 0.9f;
    }
}
