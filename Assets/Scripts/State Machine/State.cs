using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    public abstract class State : ScriptableObject
    {
        public abstract void OnEnter(Driver driver);

        public abstract void Tick(Driver driver);

        public abstract void OnExit(Driver driver);
    }
}

