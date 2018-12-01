namespace Tactical.Characters
{


    using System.Collections;
    using System.Collections.Generic;
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
                GetAndDrawPathTo(clickedWorldPos);
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