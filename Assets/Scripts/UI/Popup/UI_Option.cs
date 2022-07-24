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

        SetOption();
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
        gameObject.SetActive(false);
    }
    public void ClickApplyButton(PointerEventData data)
    {
        if(m_isChange)
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
        gameObject.SetActive(false);
    }

    GameObject m_selectObject;

    public void ClickInputKeyButton(PointerEventData data)
    {
        Debug.Log("click");
        Managers.UI.ShowPopupUI<UI_KeyInput>("UI_KeyInput");

        m_selectObject = EventSystem.current.currentSelectedGameObject;

        string l_keyCodeText = EventSystem.current.currentSelectedGameObject.transform.GetChild(0).GetComponent<Text>().text;

        GameObject.Find("UI_KeyInput").GetComponent<UI_KeyInput>().LoadCUrrentData(
            GetText((int)(Texts)Enum.Parse(typeof(Texts), $"{m_selectObject.name}Text")),
            (KeyCode)Enum.Parse(typeof(KeyCode), l_keyCodeText));
    } 


    bool m_isChange;

  
    float m_soundVolume = 1f;
    private void Update()
    {
        if(m_soundVolume != GetSlider((int)Sliders.SoundBar).value)
        {
            m_soundVolume = GetSlider((int)Sliders.SoundBar).value;

            GetText((int)Texts.SoundValue).text = $"{((int)(m_soundVolume * 100)).ToString()}%";

            GetButton((int)Buttons.ApplyButton).interactable = true;
            m_isChange = true;
        }
    }


    KeyCode m_forwardKey = KeyCode.W;
    KeyCode m_backwardKey = KeyCode.S;
    KeyCode m_leftKey = KeyCode.A;
    KeyCode m_rightKey = KeyCode.D;
    KeyCode m_evasionKeyKey = KeyCode.Space;
    KeyCode m_shootKey = KeyCode.Mouse0;
    void KeyLoad()
    {
        //현재 KeyCode 얻어올함수가 없네
    }

    void KeyApply()
    {
        //변경된 옵션 적용(팝업으로 바꿔서 다시 열면 저장 안됨)
        m_forwardKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Forward).transform.GetChild(0).GetComponent<Text>().text);
        m_backwardKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Backward).transform.GetChild(0).GetComponent<Text>().text);
        m_leftKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Left).transform.GetChild(0).GetComponent<Text>().text);
        m_rightKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Right).transform.GetChild(0).GetComponent<Text>().text);
        m_evasionKeyKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Evasion).transform.GetChild(0).GetComponent<Text>().text);
        m_shootKey = (KeyCode)Enum.Parse(typeof(KeyCode), GetButton((int)Buttons.Shoot).transform.GetChild(0).GetComponent<Text>().text);
    }

    public void InputDataAppry(string _string)
    {
        for (int i=0;i< Enum.GetValues(typeof(Buttons)).Length;i++)
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

    void SetOption()
    {
        GetButton((int)Buttons.Forward).transform.GetChild(0).GetComponent<Text>().text = m_forwardKey.ToString();
        GetButton((int)Buttons.Backward).transform.GetChild(0).GetComponent<Text>().text = m_backwardKey.ToString();
        GetButton((int)Buttons.Left).transform.GetChild(0).GetComponent<Text>().text = m_leftKey.ToString();
        GetButton((int)Buttons.Right).transform.GetChild(0).GetComponent<Text>().text = m_rightKey.ToString();
        GetButton((int)Buttons.Evasion).transform.GetChild(0).GetComponent<Text>().text = m_evasionKeyKey.ToString();
        GetButton((int)Buttons.Shoot).transform.GetChild(0).GetComponent<Text>().text = m_shootKey.ToString();
    }
}
