using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    public interface IContainsTarget
    {
        Unit CurrentTarget { get; set; }
    }
}
