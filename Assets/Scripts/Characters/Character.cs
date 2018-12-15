namespace Tactical.Characters
{


    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;
    using Tactical.Map;

    public class Character : MonoBehaviour
    {
        
        [SerializeField] protected float speed  = 4;
        [SerializeField] protected UICharacter characUI = null;
        [SerializeField] protected ATBGauge atbGauge = null;

        protected Stack<Map.Tile> path = new Stack<Map.Tile>();
        protected bool isMoving = false;

        protected Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
                characUI.SetATBPosition(Position);
            }
        }


        private void Start()
        {
            characUI.SetATBPosition(transform.position);
            atbGauge.OnValueChange += characUI.SetATBValue;
            atbGauge.ResetValue();
        }
        

        #region Private Methods
        protected void MoveToGoal()
        {
            Timing.RunCoroutine(_MoveAlongPath().CancelWith(gameObject));
        }

        private Vector3 GetCharacterCellCenter()
        {
            return MapManager.Instance.Grid.GetCellCenterWorld(MapManager.Instance.Grid.WorldToCell(transform.position));
        }
        #endregion

        #region Coroutines
        private IEnumerator<float> _MoveAlongPath()
        {
            if (path == null || path.Count == 0)
            {
                yield break;
            }
            
            atbGauge.Consume(100);

            //Instantaneous movement
            if (speed <= 0)
            {
                while (path.Count > 1)
                {
                    path.Pop();
                }
                Position = path.Pop().CenterWorld;
                atbGauge.StartReloading();
                yield break;
            }

            // currentPos = Position;
            Vector3 nextPos;
            Map.Tile nextTile;

            isMoving = true;

            //The character move from cell to cell.
            while (path.Count > 0)
            {
                nextTile = path.Pop();
                nextPos = nextTile.CenterWorld;

                if (nextPos == Position)
                {
                    continue;
                }

                yield return Timing.WaitUntilDone(Timing.RunCoroutine(_MoveTo(nextPos)));
                
                MapManager.Instance.Painter.ErasePathTileAt(nextTile.CellPos);
            }

            atbGauge.StartReloading();
            isMoving = false;
        }

        private IEnumerator<float> _MoveTo(Vector3 goalPos)
        {
            bool wasAlreadyMoving = isMoving;
            isMoving = true;

            float t = 0f;
            float step = (speed / (Position - goalPos).magnitude) * Time.fixedDeltaTime;

            while (t <= 1.0f)
            {
                t += step; // Goes from 0 to 1, incrementing by step each time
                Position = Vector3.Lerp(Position, goalPos, t);
                yield return Timing.WaitForOneFrame;
            }
            Position = goalPos;

            isMoving = wasAlreadyMoving;
        }
        
        #endregion
        

    }
}
