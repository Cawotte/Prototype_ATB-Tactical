using System.Collections;
using System.Collections.Generic;
using Tactical.Map;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapManager))]
public class MapManagerEditor : Editor
{
    public void OnSceneGUI()
    {
        Map map = ((MapManager)target).Map;

        if ( map == null || !Application.isPlaying )
        {
            //Debug.Log("map is null");
            return;
        }

        //Display map bounds.
        Handles.color = Color.blue;
        Handles.DrawLine(map.Grid.WorldToCell(map.CellBounds.min), map.Grid.WorldToCell(map.CellBounds.max));
        Handles.color = Color.red;
        Handles.DrawDottedLine(map.Bounds.min, map.Bounds.max, 2f);

        //Display a text with the index of the closest tile from the mouse position, and its world coordinates.

        Vector3 mouseWorldPos = GetMousePositionEditor();
        Map.Tile tile = map.GetTileAt(mouseWorldPos);

        //Display coordinates info
        string infoText = "";
        infoText += "\nMouse World Pos : " + mouseWorldPos;
        infoText += "\nCellPos : " + map.Grid.WorldToCell(mouseWorldPos);
        infoText += "\nTile index : " + map.GetTileIndexAt(mouseWorldPos);
        infoText += "\nTILE INFO : ";
        if ( tile != null )
        {
            infoText += tile.ToString();
        }
        else
        {
            infoText += "\nNo Tile there !";
        }
        Handles.color = Color.black;
        Handles.Label(map.Bounds.min, infoText);
    }


    private Vector3 GetMousePositionEditor()
    {

        Vector3 mousePosition = Event.current.mousePosition;
        mousePosition.y = SceneView.currentDrawingSceneView.camera.pixelHeight - mousePosition.y;
        mousePosition = SceneView.currentDrawingSceneView.camera.ScreenToWorldPoint(mousePosition);
        //mousePosition.y = -mousePosition.y;
        return mousePosition;
    }

    

}
