namespace Tactical.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TilemapProperties : MonoBehaviour
    {
        [SerializeField] private Type type;

        public Type Type
        {
            get
            {
                return type;
            }
        }
    }

    public enum Type
    {
        Water, Ground, Obstacle, None
    }
}


