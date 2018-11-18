using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager> {

    [SerializeField] private TilemapPathfinder pathfinder;
    [SerializeField] private Grid grid;

    [SerializeField] private GameObject character;

    private Stack<Vector3Int> path;

    public Stack<Vector3Int> Path
    {
        get
        {
            return path;
        }
    }
    
    
    public bool CalculatePathFromTo(Vector3 startWorldPos, Vector3 goalWorldPos)
    {
        Vector3Int goalCellPos = grid.WorldToCell(goalWorldPos);
        Vector3Int startCellPos = grid.WorldToCell(startWorldPos);

        path = pathfinder.GetPath(startCellPos, goalCellPos);
        return (path != null);
    }

    public void DrawPath()
    {
        pathfinder.DrawPath(path);
    }

    public void ErasePath()
    {
        pathfinder.ErasePath();
        path = null;
    }

    public void ErasePathTileAt(Vector3 worldPos)
    {
        pathfinder.ErasePathTileAt(grid.WorldToCell(worldPos));
    }


}
