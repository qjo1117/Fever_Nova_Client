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

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

    public void PopupMsgUpdate(string _message,float _delayTime)
    {
        Get<Text>((int)Texts.PopupMsg).text = _message;


    }

    public void PopupMsgDelayClose(float _delayTime)
    {
        // delay 있는 destroy 함수 사용
    }

}
