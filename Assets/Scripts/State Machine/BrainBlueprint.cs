using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    [CreateAssetMenu(fileName = "BrainBlueprint", menuName = "Lotl/State Machine/Brain Blueprint")]
    public class BrainBlueprint : ScriptableObject
    {
        [Range(1, 60)]
        [SerializeField] private float ticksPerSecond = 10;
        [SerializeField] private State entryPoint;
        [SerializeField] private List<Transition> anyTransitions = new();
        [SerializeField] private List<FromTransition> fromTransitions = new();

        public float TicksPerSecond => ticksPerSecond;
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
