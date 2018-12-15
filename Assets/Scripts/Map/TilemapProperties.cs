namespace Tactical.Map
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class TilemapProperties : MonoBehaviour
    {
        [SerializeField] private Map.Tile.TileType type;

        public Map.Tile.TileType Type
        {
            get
            {
                return type;
            }
        }
    }
}


