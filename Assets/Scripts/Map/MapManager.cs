namespace Tactical.Map
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class MapManager : Singleton<MapManager>
    {

        [SerializeField] private Grid grid;
        [SerializeField] private List<Tilemap> tilemaps = new List<Tilemap>();

        [SerializeField] private Tilemap tilemapPath;
        [SerializeField] private Tile pathTile;


        private TilemapPathfinder pathfinder;
        private Map map;

        public Map Map
        {
            get
            {
                return map;
            }
        }

        public Grid Grid
        {
            get
            {
                return grid;
            }
        }


        private void Awake()
        {
            map = new Map(tilemaps);
            pathfinder = new TilemapPathfinder(map);
        }


        #region Get Cells

        public TileFullData GetCell(Vector3 worldPos)
        {
            return map[grid.WorldToCell(worldPos)];
        }
        
        /// <summary>
        /// Get the world center of the cell at given coordinates
        /// </summary>
        /// <param name="cellPos"></param>
        /// <returns></returns>
        public Vector3 GetCellCenterWorld(Vector3Int cellPos)
        {
            return grid.GetCellCenterWorld(cellPos);
        }
        #endregion

        #region Pathfinding 
        public Stack<Vector3Int> FindAndGetPath(Vector3 startWorldPos, Vector3 goalWorldPos)
        {
            Vector3Int goalCellPos = Grid.WorldToCell(goalWorldPos);
            Vector3Int startCellPos = Grid.WorldToCell(startWorldPos);

            return pathfinder.GetPath(startCellPos, goalCellPos);
        }

        public Stack<Vector3> GetPathInCellCenter(Stack<Vector3Int> path)
        {
            if (path == null) return null;

            Stack<Vector3Int> reversedPath = new Stack<Vector3Int>(path);
            Stack<Vector3> pathWithCenter = new Stack<Vector3>();

            while (reversedPath.Count > 0)
            {
                pathWithCenter.Push(GetCellCenterWorld(reversedPath.Pop()));
            }

            return pathWithCenter;
        }

        public void DrawPath(Stack<Vector3Int> path)
        {
            if (path == null)
            {
                return;
            }
            Stack<Vector3Int> pathCopy = new Stack<Vector3Int>(new Stack<Vector3Int>(path)); ;
            tilemapPath.ClearAllTiles();
            while (path != null && pathCopy.Count > 0)
            {
                tilemapPath.SetTile(pathCopy.Pop(), pathTile);
            }
        }

        public void ErasePath()
        {
            tilemapPath.ClearAllTiles();
        }

        public void ErasePathTileAt(Vector3 worldPos)
        {
            tilemapPath.SetTile(Grid.WorldToCell(worldPos), null);
        }

        #endregion


    }

}
