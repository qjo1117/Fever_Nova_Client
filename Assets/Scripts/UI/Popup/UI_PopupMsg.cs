using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PopupMsg : UI_Popup
{
    enum Texts
    {
        PopupMsg
    }

    private string              m_message;
    private float               m_delayDeleteTime;

    public string Message { get => m_message; set => m_message = value; }
    public float DelayDeleteTime { get => m_delayDeleteTime; set => m_delayDeleteTime = value; }

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        PopupMsgSetting();
    }

    private void PopupMsgSetting()
    {
        Get<Text>((int)Texts.PopupMsg).text = m_message;
        Managers.Resource.Destroy(gameObject, m_delayDeleteTime);
    }

}
