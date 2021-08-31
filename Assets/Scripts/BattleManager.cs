using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private int maxEnemy;

    public int deathEnemyCount;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void UpdateDethEnemycount()
    {
        deathEnemyCount++;

        if (maxEnemy<=deathEnemyCount)
        {
            SceneStateManager.instance.PreparateStageScene(SceneName.Main);
        }
    }


}
