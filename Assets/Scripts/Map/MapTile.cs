
namespace Tactical.Map
{
    using System;
    using System.Collections.Generic;
    using Tactical.Characters;
    using UnityEngine;

    [Serializable]
    public class MapTile
    {
        public Vector3Int CellPos;
        public Vector2Int CellPos2;
        public Vector3 CenterWorld;
        public TileType Type;

        private List<Character> characters = new List<Character>();

        public List<Character> Characters { get => characters; }

        public MapTile(Vector3Int cellPos, Vector3 cellCenter, TileType type)
        {
            this.CellPos = cellPos;
            this.CellPos2 = new Vector2Int(cellPos.x, cellPos.y);
            this.CenterWorld = cellCenter;
            this.Type = type;
        }


        public bool IsWalkable()
        {
            return Type == TileType.Ground;
        }

        public bool HasCharacter(Character character)
        {
            return characters.Contains(character);
        }

        public bool HasACharacter()
        {
            return characters.Count != 0;
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