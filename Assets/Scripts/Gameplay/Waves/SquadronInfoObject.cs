using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Gameplay.Waves
{
    [System.Serializable]
    public class SquadronInfo
    {
        [SerializeField] private List<SquadronType> squadronData;

        public IReadOnlyList<SquadronType> SquadronTypes => squadronData;
    }

    [CreateAssetMenu(fileName = "SquadronInfo", menuName = "Lotl/Asset Management/Squadron Info")]
    public class SquadronInfoObject : ScriptableObject
    {
        [SerializeField] private SquadronInfo squadronInfo;
        public SquadronInfo SquadronInfo => squadronInfo;
    }
}