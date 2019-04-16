namespace Tactical.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TilemapProperties : MonoBehaviour
    {
        [SerializeField] private MapTile.TileType type;

        public MapTile.TileType Type
        {
            get
            {
                return type;
            }
        }
    }
}


