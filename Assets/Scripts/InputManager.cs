namespace Tactical.Map
{

    using Tactical;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;

    public class InputManager : MonoBehaviour
    {

        private Map map;
        
        [SerializeField] [ReadOnly]
        private MapTile selectedTile;

        private Action<MapTile> onTileSelection = null;
        private Action<MapTile> onTileDeselection = null;
        private Action<MapTile> onTileReselection = null;

        public Action<MapTile> OnTileSelection { get => onTileSelection; set => onTileSelection = value; }
        public Action<MapTile> OnTileDeselection { get => onTileDeselection; set => onTileDeselection = value; }
        public Action<MapTile> OnTileReselection { get => onTileReselection; set => onTileReselection = value; }
        public Map Map { get => map; set => map = value; }

        private void Start()
        {
            //Map = LevelManager.Instance.Map;
        }


        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickedWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                if (Map.IsInBounds(clickedWorldPos))
                {
                    MapTile deselectedTile = selectedTile; 
                    selectedTile = Map.GetTileAt(clickedWorldPos);

                    //If a new tile is selected
                    if (selectedTile != deselectedTile)
                    {
                        onTileDeselection?.Invoke(deselectedTile); //deselect old one
                        OnTileSelection?.Invoke(selectedTile); //Select new one
                    }
                    else
                    {
                        OnTileReselection?.Invoke(selectedTile); //Else reselected the current one.
                    }
                }


            }
        }
        


    }

}
