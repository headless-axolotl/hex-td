using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    private const float PausedTimeScale = 0f;
    private const float UnpausedTimeScale = 1f;

    private void Awake()
    {
        Unpause();
    }

    private void OnDestroy()
    {
        Unpause();
    }

    public void Unpause()
    {
        Time.timeScale = UnpausedTimeScale;
    }

    public void Pause()
    {
        Time.timeScale = PausedTimeScale;
    }
}
