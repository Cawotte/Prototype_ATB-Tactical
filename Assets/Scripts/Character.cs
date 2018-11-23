namespace Tactical.Characters
{


    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using MEC;

    public class Character : MonoBehaviour
    {

        [SerializeField] private Grid grid;
        [SerializeField] private float speed;

        private Action rightClickAction = null;
        private Action leftClickAction = null;

        private Vector3 clickedWorldPos;
        private float valueATB = 1f;
        private float durationATB = 2f;

        private bool isMoving = false;

        private void Awake()
        {
            leftClickAction += DrawPathToClickedPos;
            rightClickAction += MoveToGoal;
        }

        private void Start()
        {
            UIManager.Instance.SetPlayerATBPosition(transform.position, Vector3.up);
        }

        private void Update()
        {
            if (isMoving)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                clickedWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                leftClickAction?.Invoke();
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (valueATB == 1f)
                {
                    rightClickAction?.Invoke();
                }
            }

        }


        private void DrawPathToClickedPos()
        {
            MapManager.Instance.ErasePath();

            if (MapManager.Instance.CalculatePathFromTo(transform.position, clickedWorldPos))
            {
                //Debug.Log("Path found!");
                MapManager.Instance.DrawPath();
            }
            else
            {
                //Debug.Log("No path possible.");
            }
        }

        #region Private Methods
        private void MoveToGoal()
        {
            Timing.RunCoroutine(_MoveToGoal().CancelWith(gameObject));
        }

        private Vector3 GetCharacterCellCenter()
        {
            return grid.GetCellCenterWorld(grid.WorldToCell(transform.position));
        }
        #endregion
        #region Coroutines
        private IEnumerator<float> _MoveToGoal()
        {
            Stack<Vector3Int> path = MapManager.Instance.Path;

            if (path == null || path.Count == 0)
            {
                yield break;
            }

            //Instantaneous movement
            if (speed <= 0)
            {
                while (path.Count > 1)
                {
                    path.Pop();
                }
                transform.position = grid.GetCellCenterWorld(path.Pop());
                Timing.RunCoroutine(_reloadATB());
                yield break;
            }

            Vector3 currentPos;
            Vector3 nextPos;
            float step, t;

            isMoving = true;

            valueATB = 0f;
            UIManager.Instance.SetPlayerATB(valueATB);

            //The character move from cell to cell.
            while (path.Count > 0)
            {
                currentPos = GetCharacterCellCenter();
                nextPos = grid.GetCellCenterWorld(path.Pop());


                step = (speed / (currentPos - nextPos).magnitude) * Time.fixedDeltaTime;
                t = 0;
                while (t <= 1.0f)
                {
                    t += step; // Goes from 0 to 1, incrementing by step each time
                    transform.position = Vector3.Lerp(currentPos, nextPos, t);
                    UIManager.Instance.SetPlayerATBPosition(transform.position, Vector3.up);
                    yield return Timing.WaitForOneFrame;

                }
                transform.position = nextPos;
                MapManager.Instance.ErasePathTileAt(nextPos);
            }

            Timing.RunCoroutine(_reloadATB());
            isMoving = false;
        }

        private IEnumerator<float> _reloadATB()
        {
            float t = 0f;
            while (t < durationATB)
            {
                t += Time.deltaTime;
                valueATB = Mathf.Lerp(0, 1, t / durationATB);
                UIManager.Instance.SetPlayerATB(valueATB);
                yield return Timing.WaitForOneFrame;
            }

            valueATB = 1f;
            UIManager.Instance.SetPlayerATB(valueATB);
        }
        #endregion
        

    }
}
