using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ShowPopupUI로 화면을 띄우고, 프로퍼티를 이용해서 message와, delete delay time을 설정하여 사용하면된다.

public class UI_PopupMsg : UI_Popup
{
    #region UI컴포넌트_ENUM
    enum Texts
    {
        PopupMsg            
    }
    #endregion

    #region 변수
    private string  m_message;
    private float   m_delayDeleteTime;
    #endregion

    #region 프로퍼티
    public string   Message { get => m_message; set => m_message = value; }
    public float    DelayDeleteTime { get => m_delayDeleteTime; set => m_delayDeleteTime = value; }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

    private void Update()
    {
        PopupMsgUpdate();
    }

    private void PopupMsgUpdate()
    {
        Get<Text>((int)Texts.PopupMsg).text = m_message;
        Managers.Resource.Destroy(gameObject, m_delayDeleteTime);
    }

}
