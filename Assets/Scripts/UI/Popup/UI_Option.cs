using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

using Define;

public class UI_Option : UI_Popup
{
    #region ENUM
    enum Sliders
    {
        SoundBar,
    }

    enum Texts
    {
        ForwardText,
        BackwardText,
        LeftText,
        RightText,
        EvasionText,
        ShootText,

        ForwardTag,
        BackwardTag,
        LeftTag,
        RightTag,
        EvasionTag,
        ShootTag,

        SoundText,
        SoundValue,
    }

    enum Buttons
    {
        Forward,
        Backward,
        Left,
        Right,
        Evasion,
        Shoot,

        OkButton,
        ApplyButton,
        CancelButton,
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

    }

    #region ButtonClicked
    public void ClickOkButton(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            KeyApply();
            m_isInitialize = false;
            Managers.UI.ClosePopupUI();
        }
    }
    public void ClickApplyButton(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {

            if (m_isChange)
            {
                KeyApply();

                GetButton((int)Buttons.ApplyButton).interactable = false;
                m_isChange = false;
            }
        }
    }
    public void ClickCancelButton(PointerEventData data)
    {
        if(data.button == PointerEventData.InputButton.Left)
        {
            m_isInitialize = false;
            ClosePopupUI();
        }
    }

    GameObject m_selectObject;

    public void ClickInputKeyButton(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            Managers.UI.ShowPopupUI<UI_KeyInput>("UI_KeyInput");

            m_selectObject = EventSystem.current.currentSelectedGameObject;

            string l_keyCodeText = m_selectObject.transform.GetChild(0).GetComponent<Text>().text;

            GameObject.Find("UI_KeyInput").GetComponent<UI_KeyInput>().LoadCUrrentData(
                GetText((int)(Texts)Enum.Parse(typeof(Texts), $"{m_selectObject.name}Tag")),
                (KeyCode)Enum.Parse(typeof(KeyCode), l_keyCodeText));
        }
    }
    #endregion

    private bool m_isInitialize;
    private bool m_isChange;

    private float m_defaultSound = 1f;
    private float m_curSoundVolume;
    private float m_soundVolume;

    private void Update()
    {
        if (m_isInitialize == false)
        {
            SetOption();
            m_isInitialize = true;
            m_isChange = false;
            GetButton((int)Buttons.ApplyButton).interactable = false;
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
        
        Managers.Input.ChangeKey(UserKey.Forward, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.ForwardText).text));
        Managers.Input.ChangeKey(UserKey.Backward, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.BackwardText).text));
        Managers.Input.ChangeKey(UserKey.Left, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.LeftText).text));
        Managers.Input.ChangeKey(UserKey.Right, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.RightText).text));
        Managers.Input.ChangeKey(UserKey.Evasion, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.EvasionText).text));
        Managers.Input.ChangeKey(UserKey.Shoot, (KeyCode)Enum.Parse(typeof(KeyCode), GetText((int)Texts.ShootText).text));
    }

    public void InputDataAppry(string _string)
    {



        for (int i = 0; i < Enum.GetValues(typeof(UserKey)).Length; i++)
        {
            if (GetText(i).text == _string)
            {
                GetText(i).text = KeyCode.None.ToString();
            }
        }
        m_selectObject.transform.GetChild(0).GetComponent<Text>().text = _string;
        GetButton((int)Buttons.ApplyButton).interactable = true;
        m_isChange = true;
    }

    public void SetOption()
    {
        GetSlider((int)Sliders.SoundBar).value = m_soundVolume;

        GetText((int)Texts.ForwardText).text = Managers.Input.GetKeyData(UserKey.Forward).ToString();
        GetText((int)Texts.BackwardText).text = Managers.Input.GetKeyData(UserKey.Backward).ToString();
        GetText((int)Texts.LeftText).text = Managers.Input.GetKeyData(UserKey.Left).ToString();
        GetText((int)Texts.RightText).text = Managers.Input.GetKeyData(UserKey.Right).ToString();
        GetText((int)Texts.EvasionText).text = Managers.Input.GetKeyData(UserKey.Evasion).ToString();
        GetText((int)Texts.ShootText).text = Managers.Input.GetKeyData(UserKey.Shoot).ToString();


    }
}
