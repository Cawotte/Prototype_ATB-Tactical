﻿namespace Cawotte.Tactical.Level
{

    using System.Collections;
    using System.Collections.Generic;
    using Cawotte.Tactical.Level;
    using Cawotte.Utils;
    using UnityEngine;
    using UnityEngine.Tilemaps;

    public class LevelManager : Singleton<LevelManager>
    {

        [Header("Misc")]
        [SerializeField] InputManager inputManager;

        [Header("Map")]
        [SerializeField] private Grid grid;
        [SerializeField] private GameObject tilemapsParent;
        [SerializeField] private TilemapPainter painter;


        [Header("Characters")]
        [SerializeField] private Character[] characters;

        [SerializeField] [ReadOnly] Character selectedCharacter;

        private Pathfinder pathfinder;
        private Map map;
        private Stack<MapTile> path;

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
            selectedCharacter = characters[0];
            map = new Map(grid, tilemapsParent.GetComponentsInChildren<Tilemap>());
            pathfinder = new Pathfinder(map);

            if (inputManager == null)
            {
                inputManager = gameObject.AddComponent<InputManager>();
            }

            inputManager.Map = map;
        }

        private void Start()
        {
            inputManager.OnTileSelection += (tile) => LeftClick(tile);
            inputManager.OnTileReselection += (tile) => TryMovingCharacter();
        }

        private void LeftClick(MapTile tile)
        {
            if (tile.ContainsACharacter() && !tile.Contains(selectedCharacter))
            {
                selectedCharacter = (Character)tile.Content[0];
                Debug.Log("New character selected !");
                painter.EraseTiles();
            }
            else
            {
                DrawPath(selectedCharacter, tile);
            }
            

        }

        private void DrawPath(Character character, MapTile tile)
        {
            if (selectedCharacter.IsMoving) return;

            if ( map.GetTileIndexAt(character.Position) == map.GetTileIndexAt(tile.CenterWorld) )
            {
                //It's the same position
                return;
            }

            character.Path = pathfinder.GetPath(character.Position, tile.CenterWorld);

            if (!character.Path.IsEmpty)
            {
                painter.PaintTiles(character.Path.GetCellArray());
            }

        }

        private void TryMovingCharacter()
        {
            if (selectedCharacter.HasPath() && !selectedCharacter.IsMoving && selectedCharacter.AtbGauge.IsFull() )
            {
                selectedCharacter.MoveToGoal();
            }
        }

    }

}
