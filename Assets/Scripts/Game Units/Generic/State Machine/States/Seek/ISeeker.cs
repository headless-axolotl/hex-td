using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Units.Generic.StateMachine
{
    public interface ISeeker : IContainsTarget
    {
        float SeekRange { get; }
        UnitTribeMask ScanTribeMask { get; }
        LayerMask ScanMask { get; }
    }
}

