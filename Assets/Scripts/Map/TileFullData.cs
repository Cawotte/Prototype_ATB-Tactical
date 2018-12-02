namespace Tactical.Map
{


    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    using UnityEngine.Tilemaps;

    [Serializable]
    public class TileFullData 
    {
        private Vector3Int cellPos;
        private bool isWater = false;
        private bool isGround = false;
        private bool isObstacle = false;

        public TileFullData(Vector3Int cellPos, bool isWater = false, bool isGround = false, bool isObstacle = false)
        {
            this.cellPos = cellPos;
            this.isWater = isWater;
            this.isGround = isGround;
            this.isObstacle = isObstacle;
        }

        public Vector3Int CellPos
        {
            get
            {
                return cellPos;
            }
        }

        public bool IsWater
        {
            get
            {
                return isWater;
            }
        }

        public bool IsGround
        {
            get
            {
                return isGround;
            }
        }

        public bool IsObstacle
        {
            get
            {
                return isObstacle;
            }
        }

        public bool IsWalkable()
        {
            return isGround && !isObstacle;
        }
    }

}
