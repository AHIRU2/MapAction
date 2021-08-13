using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    [SerializeField]
    private Stage stage;

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

    ///// <summary>
    ///// バトルシーンへ遷移
    ///// </summary>
    //public void MoveBattleScene()
    //{
    //    SceneManager.LoadScene(SceneName.Battle.ToString());
    //}


    /// <summary>
    /// Stageシーンへの遷移準備
    /// </summary>
    /// <param name="nextLoadSceneNane"></param>
    public void PreparateStageScene(SceneName nextLoadSceneName)
    {
        StartCoroutine(LoadStageScene(nextLoadSceneName));
    }


    /// <summary>
    /// Stageシーンへの遷移
    /// </summary>
    /// <param name="nextLoadSceneName"></param>
    /// <returns></returns>
    private IEnumerator LoadStageScene(SceneName nextLoadSceneName)
    {
        string olsSceneName = SceneManager.GetActiveScene().name;
        Scene scene = SceneManager.GetSceneByName(nextLoadSceneName.ToString());

        while (!scene.isLoaded)
        {
            yield return null;
        }

        SceneManager.SetActiveScene(scene);
        stage.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync(olsSceneName);
    }


    /// <summary>
    /// Battleシーンへの遷移準備
    /// </summary>
    public void PreparateBattleScene()
    {
        Debug.Log("Load Battle Scene");
        StartCoroutine(LoadBattleScene());
    }


    /// <summary>
    /// Battleシーンへの遷移
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadBattleScene()
    {
        //今のシーンを壊さず、追加で読み込む
        SceneManager.LoadScene(SceneName.Battle.ToString(),LoadSceneMode.Additive);
        Scene scene = SceneManager.GetSceneByName(SceneName.Battle.ToString());

        //上のWhile文と同じ処理。こっちの方がちょっと重い
        yield return new WaitUntil(() => scene.isLoaded);

        stage.gameObject.SetActive(false);

        //追加で読み込んだ時、どのシーンをActiveにするか指定する
        SceneManager.SetActiveScene(scene);
    }
}
