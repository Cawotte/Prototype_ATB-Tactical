using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cawotte.Tactical.Level
{

    public class TilePath
    {
        private Stack<MapTile> path;
        private MapTile goal;

        public Stack<MapTile> Path { get => path;  }

        public MapTile Start
        {
            get => path.Peek();
        }

        public MapTile Goal
        {
            get => goal;
        }
        public bool IsEmpty
        {
            get => path == null || path.Count == 0;
        }
        /// <summary>
        /// Empty path
        /// </summary>
        public TilePath()
        {

        }

        public TilePath(Stack<MapTile> tiles)
        {
            this.path = tiles;

            if (!IsEmpty)
            {
                this.goal = new Stack<MapTile>(tiles).Peek();
            }
        }

        public Vector3Int[] ToCellArray()
        {
            return GetCellPath().ToArray();
        }

        public MapTile[] ToArray()
        {
            return path.ToArray();
        }

        public Stack<MapTile> GetReversePath()
        {
            return new Stack<MapTile>(path);
        }

        public Stack<Vector3> GetWorldPath()
        {
            Stack<Vector3> worldPath = new Stack<Vector3>();

            //Build World path from tile path
            Stack<MapTile> copyPath = new Stack<MapTile>(path); //We reverse the pile, cuz it's gonna be re-reversed
            while (copyPath.Count > 0)
            {
                worldPath.Push(copyPath.Pop().CenterWorld);
            }

            return worldPath;
        }

        public Stack<Vector3Int> GetCellPath()
        {
            Stack<Vector3Int> cellPath = new Stack<Vector3Int>();

            //Build Cell path from tile path
            Stack<MapTile> copyPath = new Stack<MapTile>(path); //We reverse the pile, cuz it's gonna be re-reversed
            while (copyPath.Count > 0)
            {
                cellPath.Push(copyPath.Pop().CellPos);
            }

            return cellPath;
        }

        public Vector3Int[] GetCellArray()
        {
            Vector3Int[] array = new Vector3Int[path.Count];
            GetCellPath().CopyTo(array, 0);
            return array;
        }
    }
}
