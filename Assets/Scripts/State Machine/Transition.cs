using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    [System.Serializable]
    public class Transition
    {
        [SerializeField] private Condition condition;
        [SerializeField] private State stateTo;

        public Transition(Transition other)
        {
            condition = other.condition;
            stateTo = other.stateTo;
        }

        public Transition(Condition condition, State stateTo)
        {
            this.condition = condition;
            this.stateTo = stateTo;
        }

        public Condition Condition => condition;
        public State StateTo => stateTo;
    }
}
