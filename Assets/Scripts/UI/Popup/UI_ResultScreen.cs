using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultScreen : UI_Popup
{
    #region UI컴포넌트_ENUM
    enum Buttons
    {
        RestartButton,          // 다시시작 버튼
        MainScreenButton        // 메인화면 버튼
    }

    enum Texts
    {
        RestartText,            // 다시시작 버튼의 텍스트 ('다시시작')
        MainScreenText          // 메인화면 버튼의 텍스트 ('메인화면')
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.RestartButton).onClick.AddListener(() => RestartButtonClicked());
        Get<Button>((int)Buttons.MainScreenButton).onClick.AddListener(() => MainScreenButtonClicked());

    }

    // 다시하기 버튼 클릭시 실행
    private void RestartButtonClicked()
    {
        // UI_Root에서 생성되어있는 UI_Result 전부 closepopup
        // (나중에 2인플레이가 되었을떄, 결과창 2개가 띄워지게 되므로 2개의 결과창을 지우기 위해 해당 작업을 수행)
        UI_Result[] uiResults = Managers.UI.Root.GetComponentsInChildren<UI_Result>();
        foreach(UI_Result item in uiResults)
        {
            item.ClosePopupUI();
        }

        ClosePopupUI();
    }

    // 메인화면 버튼 클릭시 실행
    private void MainScreenButtonClicked()
    {
        UI_Result[] uiResults = Managers.UI.Root.GetComponentsInChildren<UI_Result>();
        foreach (UI_Result item in uiResults)
        {
            item.ClosePopupUI();
        }

        Managers.Scene.LoadScene(Define.Scene.Main);
        ClosePopupUI();
    }

}
