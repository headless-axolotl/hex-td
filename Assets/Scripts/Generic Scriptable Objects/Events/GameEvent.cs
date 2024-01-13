using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Lotl.Generic.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Lotl/Generic/Event")]
    public class GameEvent : ScriptableObject
    {
        private HashSet<GameEventListener> listeners = new();

        public void Raise()
        {
            var enumeratedListeners = listeners.ToList();
            for (int i = 0; i < enumeratedListeners.Count; i++)
            {
                enumeratedListeners[i].OnEventRaised();
            }
        }

        public void RegisterListener(GameEventListener listener)
        {
            listeners.Add(listener);
        }

        public void UnregisterListener(GameEventListener listener)
        {
            listeners.Remove(listener);
        }
    }
}
