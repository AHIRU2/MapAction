using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    public int staminaPoint;

    public int hp;

    public int maxHp;

    public enum GameState
    {
        Map,
        Battle,
        GameOver,
        GameClear
    }

    public GameState currentGameState;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
