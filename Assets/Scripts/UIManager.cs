using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{

    [SerializeField]
    private Slider slider;

    [SerializeField]
    private Text txtcurrentHp;

    [SerializeField]
    private Text txtStaminaPoint;

    // Start is called before the first frame update
    void Start()
    {
        //GameData.instance.hp = GameData.instance.maxHp;

        //HPゲージの更新
        DisplayHpGauge();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //HPゲージを現在地に合わせて制御
    public void DisplayHpGauge()
    {
        txtcurrentHp.text = "HP:" + GameData.instance.hp + "/" + GameData.instance.maxHp;
        slider.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, 0.25f);
    }

    public void DisplayStaminaPoint()
    {
        txtStaminaPoint.text= GameData.instance.staminaPoint.ToString();
    }
}
