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

    // Start is called before the first frame update
    void Start()
    {
        GameData.instance.hp = GameData.instance.maxHp;

        txtcurrentHp.text = "HP:" + GameData.instance.hp + "/"+ GameData.instance.maxHp;

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
        slider.DOValue((float)GameData.instance.hp / GameData.instance.maxHp, 0.25f);
    }
}
