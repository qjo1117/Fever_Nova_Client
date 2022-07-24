using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI_MainMenu : UI_Scene
{
    #region ENUM
    enum Images
    {
        MainImage,
    }

    enum Texts
    {
        GameTitle,
    }

    enum Buttons
    {
        PlayButton,
        OptionButton,
        ExitButton,
    }



    enum GameObjects
    {
        UI_Option,
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
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(PlayButtonClicked);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OptionButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(ExitButtonClicked);

        //GetObject((int)GameObjects.UI_Option).SetActive(false);
    }

    public void PlayButtonClicked(PointerEventData data)
    {
        Debug.Log("게임시작");
        SceneManager.LoadScene("InGame");
    }

    public void OptionButtonClicked(PointerEventData data)
    {
        Debug.Log("환경설정");
        Managers.UI.ShowPopupUI<UI_Option>("UI_Option");
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
