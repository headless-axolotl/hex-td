using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lotl.Units;

namespace Lotl.Runtime
{
    [CreateAssetMenu(fileName = "AutounitRuntimeSet", menuName = "Lotl/Runtime/Sets/Autounit Runtime Set")]
    public class AutounitRuntimeSet : AutomaticallyClearedRuntimeSet<AutounitSetAdder> { }
}
