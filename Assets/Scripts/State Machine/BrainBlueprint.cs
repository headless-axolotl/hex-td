using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    [CreateAssetMenu(fileName = "BrainBlueprint", menuName = "Lotl/State Machine/Brain Blueprint")]
    public class BrainBlueprint : ScriptableObject
    {
        [SerializeField] private State entryPoint;
        [SerializeField] private List<Transition> anyTransitions = new();
        [SerializeField] private List<FromTransition> fromTransitions = new();

        public State EntryPoint => entryPoint;
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
