using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data.Users
{
    [CreateAssetMenu(fileName = "UserCookie", menuName = "Lotl/Data/User Cookie")]
    public class UserCookie : ScriptableObject
    {
        [SerializeField] private string userId;
        [SerializeField] private string password;

        public string UserId => userId;
        public string Password => password;

        public void Set(string userId, string password)
        {
            this.userId = userId;
            this.password = password;
        }
    }
}