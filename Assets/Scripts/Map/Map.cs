
namespace Tactical.Map
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;
    using System.Linq;

    [System.Serializable]
    public class Map
    {
        
        private List<TileFullData> map = new List<TileFullData>();
        private BoundsInt bounds = new BoundsInt();

        public TileFullData this[int x, int y, int z]
        {
            get
            {
                return map.SingleOrDefault( cell => 
                    cell.CellPos.x == x &&
                    cell.CellPos.y == y &&
                    cell.CellPos.z == z);
            }
        }

        public TileFullData this[int x, int y]
        {
            get
            {
                return this[x, y, 0];
            }
        }

        public TileFullData this[Vector3Int cellPos]
        {
            get
            {
                return this[cellPos.x, cellPos.y, cellPos.z];
            }
        }

        public TileFullData this[Vector2Int cellPos]
        {
            get
            {
                return this[cellPos.x, cellPos.y, 0];
            }
        }

        public Map(List<Tilemap> tilemaps)
        {
            LoadMap(tilemaps);
        }

        public void LoadMap(List<Tilemap> tilemaps)
        {
            //Get max bounds
            bounds = new BoundsInt(Vector3Int.zero, Vector3Int.zero);
            Vector3Int minBounds = bounds.min;
            Vector3Int maxBounds = bounds.max;
            foreach (Tilemap tilemap in tilemaps)
            {
                minBounds = Vector3Int.Min(bounds.min, tilemap.cellBounds.min);
                maxBounds = Vector3Int.Max(bounds.max, tilemap.cellBounds.max);
            }
            bounds.SetMinMax(minBounds, maxBounds);

            //init
            map = new List<TileFullData>();
            TileFullData tileData;

            //To iterate through all positions in the Bounds.
            BoundsInt.PositionEnumerator cellEnumerator = bounds.allPositionsWithin;
            cellEnumerator.Reset();

            //For all positions in cellEnumerator, we get the tile data and add it to the map.
            do
            {
                TilemapProperties tilemapProperties;
                bool isWater = false;
                bool isGround = false;
                bool isObstacle = false;

                foreach (Tilemap tilemap in tilemaps)
                {
                    tilemapProperties = tilemap.GetComponent<TilemapProperties>();
                    if ( tilemapProperties == null )
                    {
                        continue;
                    }
                    if (tilemapProperties.Type == Type.Water && tilemap.HasTile(cellEnumerator.Current))
                    {
                        isWater = true;
                    }
                    if (tilemapProperties.Type == Type.Ground && tilemap.HasTile(cellEnumerator.Current))
                    {
                        isGround = true;
                    }
                    if (tilemapProperties.Type == Type.Obstacle && tilemap.HasTile(cellEnumerator.Current))
                    {
                        isObstacle = true;
                    }
                }

                tileData = new TileFullData(
                    cellEnumerator.Current,
                    isWater,
                    isGround,
                    isObstacle
                    );

                map.Add(tileData);

            } while (cellEnumerator.MoveNext()); //while there's a next pos

        }
    }
}