using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Units.Towers;

namespace Lotl.Runtime
{
    [CreateAssetMenu(fileName = "TowerRuntimeSet", menuName = "Runtime/Sets/Tower Runtime Set")]
    public class TowerRuntimeSet : AutomaticallyClearedRuntimeSet<Tower> { }
}
