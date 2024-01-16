using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IContainsPosition
    {
        Vector3 CurrentPosition { get; }
    }
}
