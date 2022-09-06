using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Define;

public class UI_KeyInput : UI_Popup
{
    #region Enum
    enum Texts
    {
        MessageText,
        KeyNameText,
        KeyValueText,
    }

    enum Buttons
    {
        OkButton,
        CanselButton
    }
    #endregion

    private UI_Option m_uiOption;

    private void Start()
    {
        Init();

        m_uiOption = GameObject.Find("UI_Option").GetComponent<UI_Option>();

        m_isInitialize = false;
        m_isOnButton = false;
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkButton).gameObject.BindEvent(OnOkButtonClicked);
        GetButton((int)Buttons.CanselButton).gameObject.BindEvent(OnCanselButtonClicked);

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventdata) => { m_isOnButton = true; });

        GetButton((int)Buttons.OkButton).gameObject.AddComponent<EventTrigger>();
        GetButton((int)Buttons.CanselButton).gameObject.AddComponent<EventTrigger>();

        GetButton((int)Buttons.OkButton).GetComponent<EventTrigger>().triggers.Add(entry);
        GetButton((int)Buttons.CanselButton).GetComponent<EventTrigger>().triggers.Add(entry);

        //버그 방지(가끔 옵션창 뒤로 감)
        gameObject.GetComponent<Canvas>().sortingOrder = 15;
    }

    public void OnOkButtonClicked(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            
            m_uiOption.InputOptionAppry(GetText((int)Texts.KeyValueText).text);
            m_isInitialize = false;
            m_isOnButton = false;
            ClosePopupUI();
        }
    }

    public void OnCanselButtonClicked(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            m_isInitialize = false;
            m_isOnButton = false;
            ClosePopupUI();
        }
    }

    bool m_isInitialize;
    bool m_isOnButton;

    private KeyCode m_curInputKey;
    private string m_KeyNameText;
    private string m_KeyValueText;

    private void OnGUI()
    {
        if (m_isInitialize == false)
        {
            GetText((int)Texts.KeyNameText).text = m_KeyNameText;
            GetText((int)Texts.KeyValueText).text = m_KeyValueText;


            m_isInitialize = true;
        }

        if (Event.current.isMouse && m_isOnButton == false)
        {
            GetText((int)Texts.KeyValueText).text = (KeyCode.Mouse0 + Event.current.button).ToString();
        }

        if (Input.anyKeyDown &&
            Input.GetKey(KeyCode.None) == false &&
            Event.current.keyCode != KeyCode.None)
        {
            m_curInputKey = Event.current.keyCode;
            GetText((int)Texts.KeyValueText).text = m_curInputKey.ToString();
            return;
        }
    }

    public KeyCode GetCurrentKeyCode()
    {
        return m_curInputKey;
    }

    public void LoadCUrrentData(Text _text, KeyCode _key)
    {
        m_curInputKey = _key;
        m_KeyNameText = _text.text;
        m_KeyValueText = _key.ToString();
    }
}
