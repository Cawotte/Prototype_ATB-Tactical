using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[ExecuteInEditMode]
public class GridTest : MonoBehaviour {

    [SerializeField] private Grid grid;
    [SerializeField] private List<Tilemap> tilemaps;
    [SerializeField] private GameObject cornerMin = null;
    [SerializeField] private GameObject cornerMax = null;

    private void Update()
    {
        Bounds maxBounds = new Bounds();
        foreach (Tilemap tilemap in tilemaps)
        {
            if ( tilemap != null )
            {
                maxBounds.Encapsulate(tilemap.localBounds);
            }
        }

        if ( cornerMax != null && cornerMin != null )
        {
            cornerMin.transform.position = maxBounds.min;
            cornerMax.transform.position = maxBounds.max;
        }

        //DisplayBoundsInfo(maxBounds);
    }

    private void DisplayBoundsInfo(Bounds bounds)
    {
        string txt = "";
        txt += "Bounds.min : " + bounds.min;
        txt += "\nBounds.max : " + bounds.max;
        txt += "\nBounds.size : " + bounds.size;
        txt += "\nBounds.extends : " + bounds.extents;
        txt += "\nBounds.center : " + bounds.center;

        Debug.Log(txt);
    }
}
