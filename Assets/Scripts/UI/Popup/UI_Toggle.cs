using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Toggle : UI_Popup
{
    #region UI컴포넌트_ENUM
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
    #endregion

    #region 변수
    private bool m_isOn = false;
    #endregion

    #region 프로퍼티
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
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        // 체크박스 이미지 클릭 이벤트 등록
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

    // on상태로 변경시
    private void FlagOnFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.black;
        // flag on시 호출할 함수

        //
        Debug.Log("Toggle On");
    }

    // off상태로 변경시
    private void FlagOffFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.white;
        // flag off시 호출할 함수

        // 
        Debug.Log("Toggle false");
    }
}
