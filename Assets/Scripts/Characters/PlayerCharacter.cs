namespace Tactical.Characters
{


    using System.Collections;
    using System.Collections.Generic;
    using Tactical.Map;
    using UnityEngine;

    public class PlayerCharacter : Character
    {

        private Vector3 clickedWorldPos;

        // Update is called once per frame
        void Update()
        {
            if (isMoving)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                clickedWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                path = MapManager.Instance.Pathfinder.GetTilePath(Position, clickedWorldPos);
                MapManager.Instance.Painter.DrawPath(path);
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (atbGauge.IsFull())
                {
                    MoveToGoal();
                }
            }
        }
    }
}