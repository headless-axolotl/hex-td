using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Lotl.Data.Users;
using Lotl.Generic.Variables;

namespace Lotl.Data
{
    public class SpecialUserTriggerEvent : MonoBehaviour
    {
        [Header("Data")]
        [SerializeField] private UserCookie userCookie;
        [Header("Config")]
        [SerializeField] private List<StringReference> specialUserTriggers;
        [SerializeField] private UnityEvent triggerEvent;

        private void Start()
        {
            if (UserMatches())
            {
                triggerEvent.Invoke();
            }
        }

        private bool UserMatches()
        {
            for(int i = 0; i < specialUserTriggers.Count; i++)
            {
                if(userCookie.UserId == specialUserTriggers[i])
                    return true;
            }
            return false;
        }
    }
}