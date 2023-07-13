using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Lotl.Runtime;

namespace Lotl.AssetManagement
{
    [CreateAssetMenu(
        fileName = "PoolMemberReference",
        menuName = "Lotl/Asset Management/Prefab/References/Pool Member Reference")]
    public class PoolMemberPrefabReference : BehaviourConstrainedPrefabReference<PoolMember> { }
}
