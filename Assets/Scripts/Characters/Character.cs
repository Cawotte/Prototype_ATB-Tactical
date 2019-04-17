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

        [SerializeField] protected UICharacter characUI = null;
        [SerializeField] protected GameObject prefabCharacUI = null;

        [SerializeField] protected float speed  = 4;
        [SerializeField] private ATBGauge atbGauge = null;
        [SerializeField] private Stack<MapTile> path = new Stack<MapTile>();

        private Map map = null;
        private MapTile currentTile = null;
        private bool isMoving = false;

        private Action<Vector3> OnPositionChange = null;
        private Action<MapTile, MapTile> OnTileChange = null;


        private MapTile CurrentTile {
            get => currentTile;
            set {
                if (currentTile != null)
                {
                    currentTile.Characters.Remove(this);
                }
                currentTile = value;
                currentTile.Characters.Add(this);
            }
        }
        #region Properties
        public Vector3 Position
        {
            get
            {
                return transform.position;
            }

            set
            {
                transform.position = value;
                OnPositionChange?.Invoke(value);
            }
        }



        public Stack<MapTile> Path { get => path; set => path = value; }
        public bool IsMoving { get => isMoving; }
        public ATBGauge AtbGauge { get => atbGauge; set => atbGauge = value; }
        #endregion
        private void Start()
        {
            //No UI assigned ? Generate one from the prefab
            if (characUI == null)
            {
                characUI = Instantiate(prefabCharacUI, UIManager.Instance.WorldCanvas).GetComponent<UICharacter>();
            }

            //Get the map
            map = LevelManager.Instance.Map;
            CurrentTile = map.GetTileAt(Position);

            characUI.SetATBPosition(transform.position);

            OnPositionChange += characUI.SetATBPosition;

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

                OnTileChange?.Invoke(currentTile, nextTile);
                CurrentTile = nextTile;
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
