using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Data
{
    [Flags]
    public enum LoadSettings
    {
        User = (1 << 0),
        Towerset = (1 << 1),
        Run = (1 << 2),
    }

    public static class LoadSettingsExtensions
    {
        public static bool Contains(this LoadSettings settings, LoadSettings flags)
        {
            return (settings & flags) != 0;
        }

        public static void Set(ref LoadSettings container, LoadSettings flags, bool set = true)
        {
            if (set) container |= flags;
            else container &= ~flags;
        }
    }
}