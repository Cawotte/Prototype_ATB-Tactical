namespace Cawotte.Tactical.Level
{


    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;
    using Cawotte.Tactical.Level;
    using Cawotte.Utils;
    using Cawotte.Tactical.UI;

    public class Character : MapObject
    {
        [Header("GameObjects")]
        [SerializeField] protected UICharacter characUI = null;
        [SerializeField] protected GameObject prefabCharacUI = null;

        [Header("Informations")]
        [SerializeField] private TilePath path = new TilePath();

        private ATBGauge atbGauge = null;

        [Header("Characteristics")]
        [SerializeField] protected float speed = 4f;
        [SerializeField] protected int movement = 4;
        [SerializeField] protected float timerATB = 1f;

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



        public TilePath Path { get => path; set => path = value; }
        public bool IsMoving { get => isMoving; }
        public ATBGauge AtbGauge { get => atbGauge; set => atbGauge = value; }
        #endregion
        private void Start()
        {
            //No UI assigned ? Generate one from the prefab
            if (characUI == null)
            {
                characUI = Instantiate(prefabCharacUI, WorldUIManager.Instance.CharactersUIParent).GetComponent<UICharacter>();
            }

            //Get the map
            map = LevelManager.Instance.Map;
            CurrentTile = map.GetTileAt(Position);

            characUI.SetATBPosition(transform.position);

            OnPositionChange += characUI.SetATBPosition;

            atbGauge = new ATBGauge(timerATB);
            atbGauge.OnValueChange += characUI.SetATBValue;
            atbGauge.ResetValue();

        }


        public bool HasPath()
        {
            return !path.IsEmpty;
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
            if (path.IsEmpty)
            {
                yield break;
            }
            
            atbGauge.Consume(100);


            //Instantaneous movement
            if (speed <= 0)
            {

                Position = Path.Goal.CenterWorld;
                atbGauge.StartReloading();
                yield break;
            }

            // currentPos = Position;
            Vector3 nextPos;
            MapTile nextTile;

            Stack<MapTile>.Enumerator enumerator = path.Path.GetEnumerator();

            isMoving = true;

            //path.Pop();
            enumerator.MoveNext();

            //The character move from cell to cell.
            while (enumerator.MoveNext())
            {
                nextTile = enumerator.Current;
                nextPos = nextTile.CenterWorld;


                while (Position != nextPos)
                {
                    yield return Timing.WaitForOneFrame;
                    Position = Vector3.MoveTowards(Position, nextPos, speed * Time.fixedDeltaTime);
                }

                OnTileChange?.Invoke(currentTile, nextTile);
                CurrentTile = nextTile;
                LevelManager.Instance.Painter.EraseTileAt(nextTile.CellPos);
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
