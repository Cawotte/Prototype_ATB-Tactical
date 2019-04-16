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
        [SerializeField] private ATBGauge atbGauge = null;

        [SerializeField] private Stack<MapTile> path = new Stack<MapTile>();
        private bool isMoving = false;

        public Vector3 Position
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

        public Stack<MapTile> Path { get => path; set => path = value; }
        public bool IsMoving { get => isMoving; }
        public ATBGauge AtbGauge { get => atbGauge; set => atbGauge = value; }

        private void Start()
        {
            characUI.SetATBPosition(transform.position);
            atbGauge.OnValueChange += characUI.SetATBValue;
            atbGauge.ResetValue();
        }


        public bool HasPath()
        {
            return path != null && path.Count != 0;
        }
        public void MoveToGoal()
        {
            Timing.RunCoroutine(_MoveAlongPath().CancelWith(gameObject));
        }
        #region Private Methods


        private Vector3 GetCharacterCellCenter()
        {
            return LevelManager.Instance.Grid.GetCellCenterWorld(LevelManager.Instance.Grid.WorldToCell(transform.position));
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
            MapTile nextTile;

            isMoving = true;

            path.Pop();

            //The character move from cell to cell.
            while (path.Count > 0)
            {
                nextTile = path.Pop();
                nextPos = nextTile.CenterWorld;


                while (Position != nextPos)
                {
                    yield return Timing.WaitForOneFrame;
                    Position = Vector3.MoveTowards(Position, nextPos, speed * Time.fixedDeltaTime);
                }

                LevelManager.Instance.Painter.ErasePathTileAt(nextTile.CellPos);
            }

            atbGauge.StartReloading();
            isMoving = false;
        }

        private IEnumerator<float> _MoveTo(Vector3 goalPos)
        {

            while (Position != goalPos)
            {
                yield return Timing.WaitForOneFrame;
                Position = Vector3.MoveTowards(Position, goalPos, speed * Time.fixedDeltaTime);
            }
            
        }
        
        #endregion
        

    }
}
