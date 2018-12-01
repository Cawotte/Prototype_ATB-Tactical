using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton<MapManager> {

    [SerializeField] private TilemapPathfinder pathfinder;
    [SerializeField] private Grid grid;

    [SerializeField] private GameObject character;
    

    public Grid Grid
    {
        get
        {
            return grid;
        }

        set
        {
            grid = value;
        }
    }

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

        while (reversedPath.Count > 0 )
        {
            pathWithCenter.Push(GetCellCenterWorld(reversedPath.Pop()));
        }

        return pathWithCenter;
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
    
    public void DrawPath(Stack<Vector3Int> path)
    {
        pathfinder.DrawPath(path);
    }

    public void ErasePath()
    {
        pathfinder.ErasePath();
    }

    public void ErasePathTileAt(Vector3 worldPos)
    {
        pathfinder.ErasePathTileAt(Grid.WorldToCell(worldPos));
    }


}
