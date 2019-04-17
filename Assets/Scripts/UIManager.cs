using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {

    [SerializeField] Camera mainCamera;
    [SerializeField] Transform worldCanvas;
    [SerializeField] Slider playerATB;

    public Transform WorldCanvas { get => worldCanvas; set => worldCanvas = value; }

    public void SetPlayerATB(float value)
    {
        playerATB.value = value;
    }
    
    public void SetPlayerATBPosition(Vector3 worldPos, Vector3 offset)
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(worldPos + offset);
        playerATB.GetComponent<RectTransform>().anchoredPosition = pos;
    }
}
