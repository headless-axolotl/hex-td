using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "StateMachine/BrainBlueprint")]
    public class BrainBlueprint : ScriptableObject
    {
        [SerializeField] private List<Transition> anyTransitions = new();
        [SerializeField] private List<FromTransition> fromTransitions = new();

        public IEnumerable<Transition> AnyTransitions => anyTransitions;
        public IEnumerable<FromTransition> FromTransitions => fromTransitions;

        [System.Serializable]
        public struct FromTransition
        {
            [SerializeField] private State from;
            [SerializeField] private Transition transition;
            
            public State From => from;
            public Transition Transition => transition;
        }
    }
}
