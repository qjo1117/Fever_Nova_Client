using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_StageSelect : UI_Popup
{
    enum Images
    {
        Background
    }

    enum Buttons
    {
        StageSelect1,
        StageSelect2,
        StageSelect3,
        BackButton
    }

    enum Texts
    {
        StageText1,
        StageText2,
        StageText3,
        BackButtonText
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.BackButton).onClick.AddListener(() => { ClosePopupUI(); });
        Get<Button>((int)Buttons.StageSelect1).onClick.AddListener(Stage1Click);
        Get<Button>((int)Buttons.StageSelect2).onClick.AddListener(Stage2Click);
        Get<Button>((int)Buttons.StageSelect3).onClick.AddListener(Stage3Click);
    }

    private void Stage1Click()
    {
        Managers.Scene.LoadScene(Define.Scene.InGame_1);
    }

    // 아직 Stage 2~3은 미구현
    private void Stage2Click()
    {

    }

    private void Stage3Click()
    {

    }
}
