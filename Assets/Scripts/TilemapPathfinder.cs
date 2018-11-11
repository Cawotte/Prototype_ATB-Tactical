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

    private int[,] MapData;
    
    public void WriteType(Vector3 worldPos)
    {
        WriteType(tilemapGround.WorldToCell(worldPos));
    }

    public Stack<Vector3Int> GetPath(Vector3Int startPos, Vector3Int endPos)
    {
        Location start = new Location(startPos);
        Location goal = new Location(endPos);
        Location current = null;

        //Keep tracks of the processed locations and unprocessed neighbors. 
        List<Location> closedList = new List<Location>();
        List<Location> openList = new List<Location>();
        int g = 0;
        

        openList.Add(start);
        


        int floodStop = 0;
        while ( openList.Count > 0 && floodStop < 2000)
        {
            floodStop++;

            //Get the loc with the lowest FScore. 
            var lowest = openList.Min(loc => loc.Fscore);
            current = openList.First(loc => loc.Fscore == lowest);

            closedList.Add(current);
            openList.Remove(current);

            if ( current.HasSamePos(goal) )
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

            openList.Remove(current);
            closedList.Add(current);

            List<Vector3Int> neighbors = GetValidNeighbors(current.pos);
            g++;

            foreach (Vector3Int neighborPos in neighbors)
            {
                Location neighbor = new Location(neighborPos);

                //if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(loc => loc.HasSamePos(neighbor)) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(loc => loc.HasSamePos(neighbor)) == null)
                {
                    // compute its score, set the parent
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

        Debug.Log("flood stop = " + floodStop);
        return null;
    }

    public void DrawPath(Stack<Vector3Int> path)
    {
        tilemapPath.ClearAllTiles();
        while ( path != null && path.Count > 0)
        {
            tilemapPath.SetTile(path.Pop(), pathTile);
        }
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
    private void InitializeMapData()
    {
        //Get the furthest corners of each map to determine the size of the map :

        Vector3Int[] minCorners = new Vector3Int[2];
        minCorners[0] = tilemapGround.origin;
        minCorners[1] = tilemapObstacle.origin;
        Vector3Int minCorner = Vector3Int.Min(minCorners[0], minCorners[1]);

        Vector3Int[] maxCorners = new Vector3Int[2];
        maxCorners[0] = tilemapGround.origin;
        maxCorners[1] = tilemapObstacle.origin;
        Vector3Int maxCorner = Vector3Int.Min(maxCorners[0], maxCorners[1]);


    }

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

    private Vector3Int GetEntryWithMinValue(Dictionary<Vector3Int, float> dictionary)
    {
        float minValue = Mathf.Infinity;
        bool firstItem = true;
        Vector3Int minEntry = Vector3Int.zero;
        foreach (KeyValuePair<Vector3Int, float> entry in dictionary)
        {
            if (entry.Value < minValue)
            {
                minValue = entry.Value;
                minEntry = entry.Key;
            }
        }
        

        return minEntry;
        
    }
    private bool IsWalkable(Vector3Int cellPos)
    {
        bool tileHasGround = tilemapGround.HasTile(cellPos);
        bool tileHasObstacle = tilemapObstacle.HasTile(cellPos);
        return ( tileHasGround && !tileHasObstacle);
    }
    private class Vector3IntEqualityComparer : IEqualityComparer<Vector3Int>
    {
        public bool Equals(Vector3Int a, Vector3Int b)
        {
            if ( a.x == b.x && a.y == b.y )
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(Vector3Int vec)
        {
            int hCode = vec.x ^ vec.y;
            return hCode.GetHashCode();
        }
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

        public bool HasSamePos(Location loc)
        {
            return (pos.x == loc.pos.x && pos.y == loc.pos.y);
        }
    }
    #endregion

}
