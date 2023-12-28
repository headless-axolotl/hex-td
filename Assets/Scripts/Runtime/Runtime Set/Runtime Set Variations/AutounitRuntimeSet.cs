using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units.Towers;

namespace Lotl.Runtime
{
    [CreateAssetMenu(fileName = "AutounitRuntimeSet", menuName = "Lotl/Runtime/Sets/Autounit Runtime Set")]
    public class AutounitRuntimeSet : AutomaticallyClearedRuntimeSet<AutounitSetAdder> { }
}
