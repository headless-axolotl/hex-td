using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Utility
{
    public abstract class Initiable
    {
        private bool isInitialized;

        public bool IsInitialized
        {
            get => isInitialized;
            set
            {
                if (value) isInitialized = true;
            }
        }

        public void Initialize()
        {
            IsInitialized = true;
        }
    }

}
