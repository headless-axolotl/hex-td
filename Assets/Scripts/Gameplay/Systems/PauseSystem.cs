using Lotl.Generic.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Gameplay
{
    public class PauseSystem : MonoBehaviour
    {
        private const float PausedTimeScale = 0f;
        private const float UnpausedTimeScale = 1f;

        [Header("Events")]
        [SerializeField] private GameEvent OnPause;
        [SerializeField] private GameEvent OnUnpause;

        public static bool IsGamePaused => Time.timeScale == PausedTimeScale;

        private void Awake()
        {
            Unpause();
        }

        private void Update()
        {
            InputPause();
        }

        private void OnDestroy()
        {
            Unpause();
        }

        private void InputPause()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (IsGamePaused) Unpause();
            else Pause();
        }

        public void Pause()
        {
            Time.timeScale = PausedTimeScale;
            OnPause.Raise();
        }

        public void Unpause()
        {
            Time.timeScale = UnpausedTimeScale;
            OnUnpause.Raise();
        }
    }
}