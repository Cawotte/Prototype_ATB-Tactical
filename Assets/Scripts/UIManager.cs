using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager> {

    [SerializeField] Camera mainCamera;
    [SerializeField] Slider playerATB;

    public void SetPlayerATB(float value)
    {
        playerATB.value = value;
    }
    
    public void SetPlayerATBPosition(Vector3 worldPos)
    {
        playerATB.transform.position = Camera.main.WorldToScreenPoint(worldPos);
    }
}
