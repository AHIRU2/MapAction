using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    [SerializeField]
    private UIManager uiManager;

    private void OnEnable()
    {
        uiManager.DisplayHpGauge();
    }
}
