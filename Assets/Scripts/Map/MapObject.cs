

namespace Cawotte.Tactical.Level
{
    using Cawotte.Utils;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Base class to all entities on the map
    /// </summary>
    public abstract class MapObject : MonoBehaviour
    {
        #region members
        protected Map map;

        [SerializeField][ReadOnly]
        protected MapTile currentTile;

        public Action<Vector3> OnPositionChange = null;
        public Action<MapTile, MapTile> OnTileChange = null;
        #endregion

        #region Properties
        public MapTile CurrentTile
        {
            get => currentTile;
            
            protected set
            {
                //remove previous
                currentTile.Remove(this);

                //events
                OnTileChange?.Invoke(currentTile, value);

                currentTile = value;
                currentTile.Add(this);

            }
        }

        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            protected set
            {
                transform.position = value;
                OnPositionChange?.Invoke(value);
            }
        }

        public Vector3Int CellPos
        {
            get => currentTile.CellPos;
        }

        public Vector3 CellCenter
        {
            get => map.Grid.GetCellCenterWorld(CellPos);
        }

        #endregion

        protected void Start()
        {
            map = LevelManager.Instance.Map;
            CurrentTile = map.GetTileAt(Position);
        }
        
    }
}
