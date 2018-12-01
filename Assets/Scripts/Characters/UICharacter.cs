namespace Tactical.Characters
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class UICharacter : MonoBehaviour
    {

        [SerializeField] private Slider characterATB;
        [SerializeField] private Vector2 offsetATB;

        private Character character;

        private void Awake()
        {
            character = GetComponent<Character>();
        }

        public void SetATBValue(float value)
        {
            characterATB.value = value;
        }

        public void SetATBPosition(Vector3 worldPos)
        {
            Vector3 offset = offsetATB;
            Vector3 pos = Camera.main.WorldToScreenPoint(worldPos + offset);
            characterATB.GetComponent<RectTransform>().anchoredPosition = pos;
        }
    }
}

