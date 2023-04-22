using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Lotl.Hexgrid
{
    public static class Extensions
    {
        public static Vector2 xz(this Vector3 vector)
            => new(vector.x, vector.z);
        
        public static Vector3 xz(this Vector2 vector)
            => new(vector.x, 0, vector.y);
    }
}