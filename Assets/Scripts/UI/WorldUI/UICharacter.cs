namespace Cawotte.Tactical.UI
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using System;

    [Serializable]
    public class UICharacter : MonoBehaviour
    {

        [SerializeField] private Slider characterATB;
        [SerializeField] private Vector2 offsetATB;

        public void SetATBValue(float value)
        {
            characterATB.value = value;
        }

        public void SetATBPosition(Vector3 worldPos)
        {
            Vector3 offset = offsetATB;
            characterATB.transform.position = worldPos + offset;
        }
    }
}

