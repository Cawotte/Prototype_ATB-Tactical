using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : Singleton {

    [SerializeField] private TilemapPathfinder pathfinder;
    [SerializeField] private Tilemap tilemapRef;

    [SerializeField] private GameObject character;

    private void Update()
    {
        if ( Input.GetMouseButtonDown(0) )
        {
            
            Vector3 clickedWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int clickedCellPos = tilemapRef.WorldToCell(clickedWorldPos);
            Vector3Int characterCellPos = tilemapRef.WorldToCell(character.transform.position);
            /*
            Debug.Log(string.Format("Coords of mouse is [X: {0} Y: {0}]", clickedWorldPos.x, clickedWorldPos.y));
            //TileData tile = world.Tile((int)pos.x, (int)pos.y);
            pathfinder.WriteType(clickedWorldPos);*/

            Stack<Vector3Int> path = pathfinder.GetPath(characterCellPos, clickedCellPos);
            if ( path == null )
            {
                Debug.Log("Pas de chemin possible!");
            }
            else
            {
                Debug.Log("Draw path!");
            }
            pathfinder.DrawPath(path);
        }
    }
}
