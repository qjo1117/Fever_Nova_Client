using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PauseScreen : UI_Popup
{
    enum Images
    {
        Background,
        LabelBackground
    }

    enum Texts
    {
        MainLabel,
        ResumeButtonText,
        MainScreenButtonText
    }

    enum Buttons
    {
        ResumeButton,
        MainScreenButton
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        Get<Button>((int)Buttons.ResumeButton).onClick.AddListener(() => ResumeButtonClicked());
        Get<Button>((int)Buttons.MainScreenButton).onClick.AddListener(() => MainScrrenButtonClicked());
    }

    // 계속하기 버튼 눌렀을떄 실행
    private void ResumeButtonClicked()
    {
        ClosePopupUI();
    }

    // 메인화면 버튼 눌렀을떄 실행
    private void MainScrrenButtonClicked()
    {
        Managers.Scene.LoadScene(Define.Scene.Main);
    }

}
