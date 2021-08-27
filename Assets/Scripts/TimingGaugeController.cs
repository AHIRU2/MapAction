using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TimingGaugeController : MonoBehaviour
{
    [SerializeField]
    private Slider slider;

    [SerializeField]
    private float pointerDuration;

    private Tween tween;

    // Start is called before the first frame update
    void Start()
    {
        MoveGaugePointer();    
    }


    /// <summary>
    /// ポインターのループ移動を開始
    /// </summary>
    public void MoveGaugePointer()
    {
        tween = slider.DOValue(1.0f, pointerDuration).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }


    /// <summary>
    /// ポインターのループ移動を停止
    /// </summary>
    public void StopPointer()
    {
        tween.Kill();
    }


    /// <summary>
    /// ポインターの移動を一時停止
    /// </summary>
    /// <returns></returns>
    public IEnumerator PausePointer()
    {
        tween.Pause();

        yield return new WaitForSeconds(0.25f);

        ResumePointer();
    }


    /// <summary>
    /// ポインターの移動を再開
    /// </summary>
    public void ResumePointer()
    {
        tween.Play();
    }


    /// <summary>
    /// クリティカルの判定、クリティカルならtrue
    /// </summary>
    /// <returns></returns>
    public bool CheckCritical()
    {
        return slider.value >= 0.45f && slider.value < 0.55f ? true : false;
    }
}
