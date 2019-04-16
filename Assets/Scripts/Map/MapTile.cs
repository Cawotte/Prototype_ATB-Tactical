
namespace Tactical.Map
{
    using System;
    using UnityEngine;

    [Serializable]
    public class MapTile
    {
        public Vector3Int CellPos;
        public Vector3 CenterWorld;
        public TileType Type;

        public MapTile(Vector3Int cellPos, Vector3 cellCenter, TileType type)
        {
            this.CellPos = cellPos;
            this.CenterWorld = cellCenter;
            this.Type = type;
        }

        public bool IsWalkable()
        {
            return Type == TileType.Ground;
        }

        public override string ToString()
        {
            string txt = "";
            txt += "\nCellPos : " + CellPos;
            txt += "\nCenter in World : " + CenterWorld;
            txt += "\nType : " + Type;
            return txt;
        }

        public enum TileType
        {
            None = 0,
            Ground = 1,
            Obstacle = 2
        }
    }


}