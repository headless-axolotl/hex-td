using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Generic.Events
{
    [CreateAssetMenu(fileName = "GameEvent", menuName = "Lotl/Generic/Event")]
    public class GameEvent : ScriptableObject
    {
        private HashSet<GameEventListener> listeners = new();

        public void Raise()
        {
            foreach(GameEventListener listener in listeners)
            {
                listener.OnEventRaised();
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
