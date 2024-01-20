using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.UI;
using Lotl.Generic.Variables;
using Lotl.Generic.Events;

namespace Lotl.Gameplay
{
    public class GameEventPrompter : MonoBehaviour
    {
        [SerializeField] private Prompt prompt;
        [SerializeField] private StringReference promptQuestion;
        [SerializeField] private StringReference promptDescription;

        [SerializeField] private GameEvent gameEvent;

        public void Prompt()
        {
            prompt.Activate(promptQuestion, promptDescription, confirm: gameEvent.Raise, null);
        }
    }
}
