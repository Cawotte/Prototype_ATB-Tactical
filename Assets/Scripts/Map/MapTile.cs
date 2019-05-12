
namespace Cawotte.Tactical.Level
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using UnityEngine;

    [Serializable]
    public class MapTile
    {
        //Note : Set Tile member as [ReadOnly] to avoid modifications because of SerializeField

        #region Members
        [SerializeField]
        private Vector3Int cellPos;

        [SerializeField]
        private Vector2Int cellPos2;

        [SerializeField]
        private Vector3 centerWorld;
        
        [SerializeField]
        private TileType type;

        [SerializeField]
        private List<MapObject> content = new List<MapObject>();

        #endregion

        #region Properties
        public List<MapObject> Content { get => content; }

        public Vector3Int CellPos { get => cellPos;  }
        public Vector2Int CellPos2 { get => cellPos2; }
        public Vector3 CenterWorld { get => centerWorld; }
        public TileType Type { get => type;  }
        #endregion

        public MapTile(Vector3Int cellPos, Vector3 cellCenter, TileType type)
        {
            this.cellPos = cellPos;
            this.cellPos2 = new Vector2Int(cellPos.x, cellPos.y);
            this.centerWorld = cellCenter;
            this.type = type;
        }


        public bool IsWalkable()
        {
            return Type == TileType.Ground;
        }

        public bool Contains(MapObject mapObject)
        {
            return content.Contains(mapObject);
        }

        public bool Add(MapObject mapObject)
        {
            if (content.Contains(mapObject))
            {
                return false;
            }
            content.Add(mapObject);
            return true;
        }

        public bool Remove(MapObject mapObject)
        {
            if (!content.Contains(mapObject))
            {
                return false;
            }
            content.Remove(mapObject);
            return true;
        }

        public bool ContainsACharacter()
        {
            foreach (MapObject obj in content)
            {
                if (obj is Character)
                {
                    return true;
                }
            }
            return false;
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