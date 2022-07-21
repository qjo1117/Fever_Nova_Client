using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Toggle : UI_Popup
{
    enum Images
    {
        BackGround,
        CheckFrame,
        CheckBox,
        LabelFrame
    }

    enum Texts
    {
        ToggleLabel
    }

    private bool m_isOn = false;

    public bool OnOff
    {
        get 
        {
            return m_isOn;
        }
        set
        {
            m_isOn = value;

            if (m_isOn)
            {
                FlagOnFunction();
            }
            else
            {
                FlagOffFunction();
            }
        }
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        GameObject go = Get<Image>((int)Images.CheckBox).gameObject;
        go.BindEvent((PointerEventData data) => 
        {
            if(m_isOn)
            {
                OnOff = false;
            }
            else
            {
                OnOff = true;
            }
        }
        , Define.UIEvent.Click);
    }

    private void FlagOnFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.black;
        // flag on시 호출할 함수
        Debug.Log("Toggle On");
    }

    private void FlagOffFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.white;
        // flag off시 호출할 함수
        Debug.Log("Toggle false");
    }
}
