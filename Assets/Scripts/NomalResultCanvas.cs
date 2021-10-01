using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NomalResultCanvas : MonoBehaviour
{
    [SerializeField]
    private Text txtExp;

    [SerializeField]
    private Text txtTotalComboCount;

    [SerializeField]
    private RectTransform imgBackFrameRect;

    [SerializeField]
    private CanvasGroup canvasGroupResult;

    [SerializeField]
    private CanvasGroup canvasGroupExpSet;

    [SerializeField]
    private CanvasGroup canvasGroupTotalComboCountSet;


    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="exp"></param>
    /// <param name="totalComboCount"></param>
    public void DisplayResult(int exp,int totalComboCount)
    {
        //一旦全て非表示にする
        canvasGroupResult.alpha = 0;
        canvasGroupExpSet.alpha = 0;
        canvasGroupTotalComboCountSet.alpha = 0;
        imgBackFrameRect.sizeDelta = new Vector2(0, 355);

        Sequence sequence = DOTween.Sequence();

        //Canvas表示。この時点ではResultの文字だけでる
        sequence.Append(canvasGroupResult.DOFade(1.0f, 0.5f).SetEase(Ease.Linear));

        //リザルトの表示用のフレームをアニメ表示（縦方向に伸ばす）
        sequence.Append(imgBackFrameRect.DOSizeDelta(new Vector2(1200, 355), 0.5f).SetEase(Ease.OutQuart));

        //Exp表示
        sequence.Append(canvasGroupExpSet.DOFade(1.0f, 0.5f));

        //Exp加算アニメ
        sequence.Append(txtExp.DOCounter(0, exp, 0.5f).SetEase(Ease.InQuart));

        //コンボ数表示
        sequence.Append(canvasGroupTotalComboCountSet.DOFade(1.0f, 0.5f));

        //コンボ数加算アニメ
        sequence.Append(txtTotalComboCount.DOCounter(0, totalComboCount, 0.5f).SetEase(Ease.InQuart));
    }
}
