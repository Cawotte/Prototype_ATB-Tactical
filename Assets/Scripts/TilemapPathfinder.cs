using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TilemapPathfinder : MonoBehaviour {

    [SerializeField] private Tilemap tilemapGround;
    [SerializeField] private Tilemap tilemapObstacle;

    [SerializeField] private Tilemap tilemapPath;
    [SerializeField] private Tile pathTile;

    //private int[,] MapData;
    
    public void WriteType(Vector3 worldPos)
    {
        WriteType(tilemapGround.WorldToCell(worldPos));
    }

    public Stack<Vector3Int> GetPath(Vector3Int startPos, Vector3Int endPos)
    {
        Location start = new Location(startPos);
        Location goal = new Location(endPos);
        Location current = null;

        if ( start.EqualsTo(goal) )
        {
            return new Stack<Vector3Int>(); //return empty path.
        }

        //Keep tracks of the processed locations and unprocessed neighbors. 
        List<Location> closedList = new List<Location>();
        List<Location> openList = new List<Location>();
        int g = 0;
        
        openList.Add(start);
        
        while ( openList.Count > 0 )
        {

            //Get the unprocessed location with the lowest FScore. 
            var lowest = openList.Min(loc => loc.Fscore);
            current = openList.First(loc => loc.Fscore == lowest);

            closedList.Add(current);
            openList.Remove(current);

            //If current is the goal cell.
            if ( current.EqualsTo(goal) )
            {
                //get final path
                Stack<Vector3Int> path = new Stack<Vector3Int>();
                do
                {
                    path.Push(current.pos);
                    current = current.Parent;
                } while (current.Parent != null);

                return path;
            }

            //We move the location from unprocessed to processed.
            openList.Remove(current);
            closedList.Add(current);

            List<Vector3Int> neighbors = GetValidNeighbors(current.pos);
            g++;

            foreach (Vector3Int neighborPos in neighbors)
            {
                Location neighbor = new Location(neighborPos);

                //if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(loc => loc.EqualsTo(neighbor)) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(loc => loc.EqualsTo(neighbor)) == null)
                {
                    // compute its scores, set the parent
                    neighbor.Gscore = g;
                    neighbor.Hscore = (int)Vector3Int.Distance(neighbor.pos, goal.pos);
                    neighbor.Fscore = neighbor.Gscore + neighbor.Hscore;
                    neighbor.Parent = current;

                    // and add it to the open list
                    openList.Insert(0, neighbor);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (g + neighbor.Hscore < neighbor.Fscore)
                    {
                        neighbor.Gscore = g;
                        neighbor.Fscore = neighbor.Gscore + neighbor.Hscore;
                        neighbor.Parent = current;
                    }
                }
                
            }

        }

        //Debug.Log("There's no path possible to go there.");
        return null;
    }

    public void DrawPath(Stack<Vector3Int> path)
    {
        if ( path == null )
        {
            return;
        }
        Stack<Vector3Int> pathCopy = new Stack<Vector3Int>(new Stack<Vector3Int>(path)); ;
        tilemapPath.ClearAllTiles();
        while ( path != null && pathCopy.Count > 0)
        {
            tilemapPath.SetTile(pathCopy.Pop(), pathTile);
        }
    }

    public void ErasePath()
    {
        tilemapPath.ClearAllTiles();
    }

    public void ErasePathTileAt(Vector3Int cellPos)
    {
        tilemapPath.SetTile(cellPos, null);
    }

    public void WriteType(Vector3Int cellPos)
    {
        bool tileHasGround = tilemapGround.HasTile(cellPos);
        bool tileHasObstacle = tilemapObstacle.HasTile(cellPos);

        if (tileHasGround)
        {
            Debug.Log("This tile has a ground.");
        }
        if ( tileHasObstacle)
        {
            Debug.Log("This tile has an obstacle.");
        }
    }

    #region private Methods

    private List<Vector3Int> GetValidNeighbors(Vector3Int cellPos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>();

        if ( IsWalkable(cellPos + Vector3Int.up) )
        {
            neighbors.Add(cellPos + Vector3Int.up);
        }
        if (IsWalkable(cellPos + Vector3Int.down))
        {
            neighbors.Add(cellPos + Vector3Int.down);
        }
        if (IsWalkable(cellPos + Vector3Int.left))
        {
            neighbors.Add(cellPos + Vector3Int.left);
        }
        if (IsWalkable(cellPos + Vector3Int.right))
        {
            neighbors.Add(cellPos + Vector3Int.right);
        }

        return neighbors;
    }
    
    private bool IsWalkable(Vector3Int cellPos)
    {
        bool tileHasGround = tilemapGround.HasTile(cellPos);
        bool tileHasObstacle = tilemapObstacle.HasTile(cellPos);
        return ( tileHasGround && !tileHasObstacle);
    }

    private class Location
    {
        public Vector3Int pos;
        public int Fscore, Gscore, Hscore;
        public Location Parent = null;

        public Location(Vector3Int pos)
        {
            this.pos = pos;
        }

        public bool EqualsTo(Location loc)
        {
            return (pos.x == loc.pos.x && pos.y == loc.pos.y);
        }
    }
    #endregion

}
