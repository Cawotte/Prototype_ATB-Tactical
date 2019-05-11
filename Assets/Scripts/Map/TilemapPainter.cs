namespace Cawotte.Tactical.Level
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    [System.Serializable]
    public class TilemapPainter
    {
        
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private Tile defaultTile;
        
        public void PaintTiles(Vector3Int[] cells)
        {

            //Stack <Vector3Int> cellPath = Pathfinder.GetCellPath(path);
            
            TileBase[] tiles = new TileBase[cells.Length];

            tilemap.ClearAllTiles();
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i] = defaultTile;
            }

            tilemap.SetTiles(cells, tiles);

        }

        public void EraseTiles()
        {
            tilemap.ClearAllTiles();
        }

        public void EraseTileAt(Vector3Int cellPos)
        {
            tilemap.SetTile(cellPos, null);
        }
    }
}
