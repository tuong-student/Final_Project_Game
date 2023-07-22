using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public static class Extension 
    {
        public static Vector3 ToVector3(this Vector2 source)
        {
            return new Vector3(source.x, source.y, 0);
        }
        public static Vector3 ToVector3XZ(this Vector2 source)
        {
            return new Vector3(source.x, 0, source.y);
        }
    }

}
