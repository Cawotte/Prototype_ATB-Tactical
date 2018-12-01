namespace Tactical.Characters
{


    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;

    public class Character : MonoBehaviour
    {
        
        [SerializeField] protected float speed;
        [SerializeField] protected UICharacter characUI = null;
        [SerializeField] protected ATBGauge atbGauge;

        protected Stack<Vector3Int> movePath = new Stack<Vector3Int>();

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
        

        protected void GetAndDrawPathTo(Vector3 goalWorldPos)
        {
            MapManager.Instance.ErasePath();

            movePath = MapManager.Instance.FindAndGetPath(Position, goalWorldPos);
            if (movePath != null && movePath.Count > 0)
            {
                MapManager.Instance.DrawPath(movePath);
            }
        }

        #region Private Methods
        protected void MoveToGoal()
        {
            Timing.RunCoroutine(_MoveToGoal().CancelWith(gameObject));
        }

        private Vector3 GetCharacterCellCenter()
        {
            return MapManager.Instance.Grid.GetCellCenterWorld(MapManager.Instance.Grid.WorldToCell(transform.position));
        }
        #endregion

        #region Coroutines
        private IEnumerator<float> _MoveToGoal()
        {

            if (movePath == null || movePath.Count == 0)
            {
                yield break;
            }

            Stack<Vector3> path = MapManager.Instance.GetPathInCellCenter(movePath);

            atbGauge.Consume(100);

            //Instantaneous movement
            if (speed <= 0)
            {
                while (path.Count > 1)
                {
                    path.Pop();
                }
                Position = path.Pop();
                atbGauge.StartReloading();
                movePath = null;
                yield break;
            }

            Vector3 currentPos = GetCharacterCellCenter();
            Vector3 nextCellPos;
            float step, t;

            isMoving = true;

            //The character move from cell to cell.
            while (path.Count > 0)
            {
                nextCellPos = path.Pop();

                //Step allow to scale timer on speed.
                step = (speed / (currentPos - nextCellPos).magnitude) * Time.fixedDeltaTime;
                t = 0;
                while (t <= 1.0f)
                {
                    t += step; // Goes from 0 to 1, incrementing by step each time
                    Position = Vector3.Lerp(currentPos, nextCellPos, t);
                    yield return Timing.WaitForOneFrame;
                }

                currentPos = Position = nextCellPos;
                MapManager.Instance.ErasePathTileAt(nextCellPos);
            }

            atbGauge.StartReloading();
            movePath = null;
            isMoving = false;
        }
        
        #endregion
        

    }
}
