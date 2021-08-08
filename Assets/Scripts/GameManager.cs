using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public Text staminaPoint;


    // Start is called before the first frame update
    void Start()
    {
        staminaPoint.text = GameData.instance.staminaPoint.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
