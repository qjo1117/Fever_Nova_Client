using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 현재 프레임워크에서는 작동 안함, 지연 삭제 함수가 존재하지 않음
// 다음 버전 프레임워크에서는 정상적으로 작동함

// 주의점! Ingame Scene내의 Init함수에서 PopupMsgSetting 함수를 호출할경우
// UI 컴포넌트 Bind가 완료되지 않은 상태므로 오류가 발생함

// 이것은 후에 MonsterManager에서 Monster 스폰시킬때 PopupMsg를 띄우면 해결됨
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
    public string   Message { get => m_message;}
    public float    DelayDeleteTime { get => m_delayDeleteTime;}
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

    private void PopupMsgSetting(string _message, float _delayDeleteTime)
    {
        m_message = _message;
        m_delayDeleteTime = _delayDeleteTime;

        Get<Text>((int)Texts.PopupMsg).text = _message;
        Managers.Resource.Destroy(gameObject, _delayDeleteTime);
    }

}
