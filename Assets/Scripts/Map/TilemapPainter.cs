namespace Cawotte.Tactical.Level
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [System.Serializable]
    public class TilemapPainter
    {

        [SerializeField] private Grid grid;
        [SerializeField] private Tilemap pathTilemap;
        [SerializeField] private Tile pathTile;

        public TilemapPainter(Grid grid)
        {
            this.grid = grid;
        }

        public void DrawPath(Stack<MapTile> path)
        {
            if (path == null)
            {
                return;
            }
            Stack<Vector3Int> cellPath = Pathfinder.GetCellPath(path);
            pathTilemap.ClearAllTiles();
            while (cellPath.Count > 0)
            {
                pathTilemap.SetTile(cellPath.Pop(), pathTile);
            }
        }

        public void ErasePath()
        {
            pathTilemap.ClearAllTiles();
        }

        public void ErasePathTileAt(Vector3Int cellPos)
        {
            pathTilemap.SetTile(cellPos, null);
        }
    }
}
