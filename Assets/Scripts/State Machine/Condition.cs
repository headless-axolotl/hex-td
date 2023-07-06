using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.StateMachine
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool IsMet(Driver driver);
    }
}
