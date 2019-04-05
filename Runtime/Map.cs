using System;
using System.Collections.Generic;
using UnityEngine;

namespace Popcron.Input
{
    [CreateAssetMenu(menuName = "Controls/Map")]
    public class Map : ScriptableObject
    {
        public List<MapBind> binds = new List<MapBind>();

        public MapBind GetBind(string name)
        {
            for (int i = 0; i < binds.Count; i++)
            {
                if (binds[i].name == name)
                {
                    return binds[i];
                }
            }

            return null;
        }
    }
}