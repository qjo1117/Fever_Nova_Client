using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI_MainMenu : UI_Scene
{
    #region ENUM
    enum Images
    {
        MainImage,
    }

    enum TextMeshPros
    {
        GameTitle,
    }

    enum Buttons
    {
        PlayButton,
        OptionButton,
        ExitButton,
    }

    #endregion

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(TextMeshPros));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(PlayButtonClicked);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OptionButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(ExitButtonClicked);

        //GetObject((int)GameObjects.UI_Option).SetActive(false);
    }

    public void PlayButtonClicked(PointerEventData data)
    {
        Debug.Log("게임시작");
        Managers.Scene.LoadScene(Define.Scene.InGame);
    }

    public void OptionButtonClicked(PointerEventData data)
    {
        Debug.Log("환경설정");
        Managers.UI.ShowPopupUI<UI_Option>("UI_Option");
        //GameObject.Find("UI_Option").GetComponent<UI_Option>().SetOption();
    }

    public void ExitButtonClicked(PointerEventData data)
    {
        //종료버튼 누르면 바로 종료
#if UNITY_EDITOR
        Debug.Log("게임 종료");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
