namespace Tactical.Map
{

    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class MapManager : Singleton<MapManager>
    {

        [SerializeField] private Grid grid;
        [SerializeField] private GameObject tilemapsParent;

        [SerializeField] private TilemapPainter painter;


        private Pathfinder pathfinder;
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

        public Pathfinder Pathfinder
        {
            get
            {
                return pathfinder;
            }
        }

        public TilemapPainter Painter
        {
            get
            {
                return painter;
            }
        }

        private void Awake()
        {
            
            map = new Map(grid, tilemapsParent.GetComponentsInChildren<Tilemap>());
            pathfinder = new Pathfinder(map);
        }



        #region Pathfinding 
        /*
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
        } */
        /*
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
        } */

        #endregion


    }

}
