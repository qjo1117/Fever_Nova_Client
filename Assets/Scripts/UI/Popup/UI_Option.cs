using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Define;

//**생각중
//키 입력시 하는 순간
//현재 사용하는 키와 중복되는지 확인
//중복되거나(아니면 무조건)
//변경할 건지 확인하고 이전에 설정된 키 해제 후 변경

//옵션에 변경이 생겼을 때만 적용버튼 활성화
//적용후 비활성화


public class UI_Option : UI_Popup
{
    #region ENUM
    enum Sliders
    {
        SoundBar,
    }

    enum Texts
    {
        SoundText,
        SoundValue,

        ForwardText,
        BackwardText,
        LeftText,
        RightText,
        EvasionText,
        ShootText,
    }

    enum Buttons
    {
        OkButton,
        ApplyButton,
        CancelButton,

        Forward,
        Backward,
        Left,
        Right,
        Evasion,
        Shoot,
    }

    #endregion

    private void Start()
    {
        Init();

        m_isInitialize = false;
        m_soundVolume = m_defaultSound;
        m_curSoundVolume = m_defaultSound;
    }

    public override void Init()
    {
        base.Init();

        Bind<Slider>(typeof(Sliders));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkButton).gameObject.BindEvent(ClickOkButton);
        GetButton((int)Buttons.ApplyButton).gameObject.BindEvent(ClickApplyButton);
        GetButton((int)Buttons.CancelButton).gameObject.BindEvent(ClickCancelButton);

        GetButton((int)Buttons.Forward).gameObject.BindEvent(ClickInputKeyButton);
        GetButton((int)Buttons.Backward).gameObject.BindEvent(ClickInputKeyButton);
        GetButton((int)Buttons.Left).gameObject.BindEvent(ClickInputKeyButton);
        GetButton((int)Buttons.Right).gameObject.BindEvent(ClickInputKeyButton);
        GetButton((int)Buttons.Evasion).gameObject.BindEvent(ClickInputKeyButton);
        GetButton((int)Buttons.Shoot).gameObject.BindEvent(ClickInputKeyButton);


        GetButton((int)Buttons.ApplyButton).interactable = false;
        m_isChange = false;
    }

    public void ClickOkButton(PointerEventData data)
    {
        Debug.Log("확인");
        KeyApply();
        m_isInitialize = false;
        Managers.UI.ClosePopupUI();
    }
    public void ClickApplyButton(PointerEventData data)
    {
        if (m_isChange)
        {
            Debug.Log("적용");
            KeyApply();

            GetButton((int)Buttons.ApplyButton).interactable = false;
            m_isChange = false;
        }
    }
    public void ClickCancelButton(PointerEventData data)
    {
        Debug.Log("취소");
        m_isInitialize = false;
        ClosePopupUI();
    }

    GameObject m_selectObject;

    public void ClickInputKeyButton(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)//좌클릭 일때만 실행
        {
            Debug.Log("click");
            Managers.UI.ShowPopupUI<UI_KeyInput>("UI_KeyInput");

            m_selectObject = EventSystem.current.currentSelectedGameObject;

            string l_keyCodeText = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;

            GameObject.Find("UI_KeyInput").GetComponent<UI_KeyInput>().LoadCUrrentData(
                GetText((int)(Texts)Enum.Parse(typeof(Texts), $"{m_selectObject.name}Text")),
                (KeyCode)Enum.Parse(typeof(KeyCode), l_keyCodeText));
        }
    }

    bool m_isInitialize;
    bool m_isChange;

    float m_defaultSound = 1f;
    float m_curSoundVolume;
    float m_soundVolume;
    private void Update()
    {
        if (m_isInitialize == false)
        {
            SetOption();
            m_isInitialize = true;
        }

        if (m_curSoundVolume != GetSlider((int)Sliders.SoundBar).value)
        {
            m_curSoundVolume = GetSlider((int)Sliders.SoundBar).value;

            GetText((int)Texts.SoundValue).text = $"{((int)(m_curSoundVolume * 100)).ToString()}%";

            GetButton((int)Buttons.ApplyButton).interactable = true;
            m_isChange = true;
        }
    }


    List<KeyCode> m_usingKey;

    void LoadUsingKey()
    {
        foreach (var item in (UserKey[])Enum.GetValues(typeof(UserKey)))
        {
            m_usingKey.Add(Managers.Input.GetKeyData(item));
        }
    }

    void KeyApply()
    {
        //변경된 옵션 적용(팝업으로 바꿔서 다시 열면 저장 안됨)
        m_soundVolume = m_curSoundVolume;

        Managers.Input.ChangeKey(UserKey.Forward, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Forward).transform.GetChild(0).GetComponent<Text>().text));
        Managers.Input.ChangeKey(UserKey.Backward, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Backward).transform.GetChild(0).GetComponent<Text>().text));
        Managers.Input.ChangeKey(UserKey.Left, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Left).transform.GetChild(0).GetComponent<Text>().text));
        Managers.Input.ChangeKey(UserKey.Right, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Right).transform.GetChild(0).GetComponent<Text>().text));
        Managers.Input.ChangeKey(UserKey.Evasion, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Evasion).transform.GetChild(0).GetComponent<Text>().text));
        Managers.Input.ChangeKey(UserKey.Shoot, (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Shoot).transform.GetChild(0).GetComponent<Text>().text));
    }

    public void InputDataAppry(string _string)
    {
        for (int i = 0; i < Enum.GetValues(typeof(Buttons)).Length; i++)
        {
            if (GetButton(i).transform.GetChild(0).GetComponent<Text>().text == _string)
            {
                GetButton(i).transform.GetChild(0).GetComponent<Text>().text = KeyCode.None.ToString();
            }
        }
        m_selectObject.transform.GetChild(0).GetComponent<Text>().text = _string;
        GetButton((int)Buttons.ApplyButton).interactable = true;
        m_isChange = true;
    }

    public void SetOption()
    {
        GetSlider((int)Sliders.SoundBar).value = m_soundVolume;

        GetButton((int)Buttons.Forward).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Forward).ToString();
        GetButton((int)Buttons.Backward).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Backward).ToString();
        GetButton((int)Buttons.Left).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Left).ToString();
        GetButton((int)Buttons.Right).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Right).ToString();
        GetButton((int)Buttons.Evasion).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Evasion).ToString();
        GetButton((int)Buttons.Shoot).transform.GetChild(0).GetComponent<Text>().text = Managers.Input.GetKeyData(UserKey.Shoot).ToString();
    }
}
