using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using Define;

public class UI_KeyInput : UI_Popup
{
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

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OkButton).gameObject.BindEvent(OnOkButtonClicked);
        GetButton((int)Buttons.CanselButton).gameObject.BindEvent(OnCanselButtonClicked);

        GetText((int)Texts.KeyNameText).text = m_KeyNameText;
        GetText((int)Texts.KeyValueText).text = m_KeyValueText;

    }

    public void OnOkButtonClicked(PointerEventData data)
    {
        GameObject.Find("UI_Option").GetComponent<UI_Option>().InputDataAppry(GetText((int)Texts.KeyValueText).text);
        ClosePopupUI();
    }

    public void OnCanselButtonClicked(PointerEventData data)
    {
        ClosePopupUI();
    }

    private KeyCode m_curInputKey;
    private string m_KeyNameText;
    private string m_KeyValueText;
    private void OnGUI()
    {
        if (Input.anyKeyDown)
        {
            if(m_curInputKey != Event.current.keyCode &&
                Event.current.keyCode != KeyCode.None)
            {
                m_curInputKey = Event.current.keyCode;

                if (m_curInputKey == KeyCode.None||
                    m_curInputKey == KeyCode.LeftAlt ||
                    m_curInputKey == KeyCode.RightAlt ||
                    m_curInputKey == KeyCode.LeftControl ||
                    m_curInputKey == KeyCode.RightControl ||
                    m_curInputKey == KeyCode.LeftShift ||
                    m_curInputKey == KeyCode.RightShift)
                {
                }
                else
                {
                    if(Input.GetKey(KeyCode.LeftShift))
                    {
                        //Debug.Log($"{KeyCode.LeftShift} + {m_curInputKey}");
                        //GetText((int)Texts.KeyValueText).text=$"{KeyCode.LeftShift} + {m_curInputKey}";
                    }
                    else
                    {
                        Debug.Log(m_curInputKey);
                        GetText((int)Texts.KeyValueText).text = m_curInputKey.ToString();
                    }
                }
            }
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
