using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    public abstract class Transition : ScriptableObject
    {
        [SerializeField] private State to;
        
        public State To => to;

        public abstract bool Check(Driver driver);
    }
}
