using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GFramework;

public class StageSelectDetail : MonoBehaviour
{
    [SerializeField]
    private Text txtStageSelect;

    [SerializeField]
    private Button btnStageSelectDetail;

    [SerializeField]
    private SimpleRoundedImage imgStageView;

    private StageDataSO.StageData stageData;

    private World world;


    public void SetUpStageSelectDetail(StageDataSO.StageData stageData,World world)
    {
        this.stageData = stageData;
        this.world = world;

        //各項目の設定
        txtStageSelect.text = this.stageData.stageName;
        imgStageView.sprite = this.stageData.stageView;
        btnStageSelectDetail.onClick.AddListener(OnClickStageSelectDetail);
    }

    private void OnClickStageSelectDetail()
    {
        //キャラのアイコンをボタン上に配置
        world.SetPlayerTran(stageData.playerIconTran);

        //選択しているステージの情報を更新
        GameData.instance.chooseStageNo = stageData.stageNo;
    }

    public void SwitchActivateButton(bool isSwitch)
    {
        btnStageSelectDetail.gameObject.SetActive(isSwitch);
    }

}
