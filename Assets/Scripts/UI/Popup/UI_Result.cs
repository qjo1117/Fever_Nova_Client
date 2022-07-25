using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result : UI_Popup
{
    #region UI컴포넌트_ENUM
    enum Images
    {
        ResultScreen_Background,
        BorderLine1,
        BorderLine2,

        ScoreLabel_Background,
        PlayTimeLabel_Background,
        KillCountLabel_Background,
        MultiKillLabel_Background,
        HitCountLabel_Background,
        TotalScoreLabel_Background,

        ResultText_Background,
        Score_Background,
        PlayTime_Background,
        KillCount_Background,
        MultiKill_Background,
        HitCount_Background,
        TotalScore_Background
    }

    enum Texts
    {
        PlayerText,
        ScoreLabel,
        PlayTimeLabel,
        KillCountLabel,
        MultiKillLabel,
        HitCountLabel,
        TotalScoreLabel,

        ResultText,
        ScoreText,
        PlayTimeText,
        KillCountText,
        MultiKillText,
        HitCountText,
        TotalScoreText
    }
    #endregion;

    #region 변수

    private int     m_playerId;
    private string  m_result;
    private int     m_score;
    private float   m_gameStartTime;
    private int     m_killCount;
    private int     m_multiKillCount;
    private int     m_hitCount;
    private int     m_totalScore;

    private int     m_strCheckBitFlag;
    private int     m_intCheckBitFlag;
    private int     m_floatCheckBitFlag;

    // m_isReday 변수가 필요한 이유
    // => 현재 오브젝트 생성방식이 비동기 방식이기때문에,ShowPopup함수를 통해 UI를 생성해도
    //    바로 Init함수가 호출되지 않아 UI 컴포넌트 Bind작업이 제대로 이루어지지 않는다.
    //    이상태에서 바로 프로퍼티를 통해 값을 넣게되면, 각 값의Update함수에서 오류가 발생할것이다.

    //    이를 막기위해 m_isReady (Bind 작업이 완료되었는가?)bool 값을 통해 Update함수에서 
    //    bind작업이 되지 않았을때 (아직 Init함수가 호출되지 않았을떄) Update 함수를 return시켜 종료하고
    //    해당 값들의 Update 작업을 Init함수에서 Bind가 완료되고 m_isReady가 true로 변경된 뒤에 수행하여
    //    정상적으로 Update 시키기위해서 m_isReady변수가 필요한것이다.

    private bool    m_isReady;              // UI 컴포넌트 Bind 작업이 완료되었는가?
    #endregion

    #region 프로퍼티
    // 기본적으로 프로퍼티에 값을 셋팅하면, 해당 값을 Update하는 함수가 호출되어 바로 UI에 반영됨

    public int PlayerId
    {
        get
        {
            return m_playerId;
        }
        set
        {
            m_playerId = value;
            StringDataTextUpdate(string.Format("(Player{0})", m_playerId), Texts.PlayerText);
        }
    }


    public string Result
    {
        get
        {
            return m_result;
        }
        set
        {
            m_result = value;
            StringDataTextUpdate(m_result, Texts.ResultText);
        }
    }

    public int Score
    {
        get
        {
            return m_score;
        }
        set
        {
            m_score = value;
            IntDataTextUpdate(m_score, Texts.ScoreText);
        }
    }

    public float GameStartTime
    {
        get
        {
            return m_gameStartTime;
        }
        set
        {
            m_gameStartTime = value;
            FloatDataTextUpdate(m_gameStartTime, Texts.PlayTimeText);
        }
    }

    public int KillCount
    {
        get
        {
            return m_killCount;
        }
        set
        {
            m_killCount = value;
            IntDataTextUpdate(m_killCount, Texts.KillCountText);
        }
    }

    public int MultiKillCount
    {
        get
        {
            return m_multiKillCount;
        }
        set
        {
            m_multiKillCount = value;
            IntDataTextUpdate(m_multiKillCount, Texts.MultiKillText);
        }
    }

    public int HitCount
    {
        get
        {
            return m_hitCount;
        }
        set
        {
            m_hitCount = value;
            IntDataTextUpdate(m_hitCount, Texts.HitCountText);
        }
    }

    public int TotalScore
    {
        get
        {
            return m_totalScore;
        }
        set
        {
            m_totalScore = value;
            IntDataTextUpdate(m_totalScore, Texts.TotalScoreText);
        }
    }
    #endregion


    public override void Init()
    {
        base.Init();

        BitFlagCreate();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        m_isReady = true;
        AllUpdate();
    }

    // IntDataTextUpdate,StringDataTextUpdate... 같은 값을 업데이트 하는함수를 확장성을 위하여 Texts enum값을 받아서
    // enum값에 해당하는 Text를 변경하는 식으로 구조를 짰다.

    // 이런경우 IntData 사용하는 UI를 업데이트하는 IntDataTextUpdate함수를 호출하는데 floatData를 사용하는 UI에대한 enum값을 매개변수로 전달하는
    // 경우가 있을 수 있기 때문에 이를 방지하기위해 비트플래그 체크를 사용하였다.
    private void BitFlagCreate()
    {
        // string 데이터 확인 비트플래그
        m_strCheckBitFlag = (int)Texts.PlayerText | (int)Texts.ResultText;

        // int 데이터 확인 비트플래그
        m_intCheckBitFlag = (int)Texts.ScoreText | (int)Texts.KillCountText | (int)Texts.MultiKillText |
            (int)Texts.HitCountText | (int)Texts.TotalScoreText;

        // float 데이터 확인 비트플래그
        m_floatCheckBitFlag = (int)Texts.PlayTimeText;
    }

    // Bind되기전 프로퍼티를 통해 입력된 값들을 일괄 Update하기 위한 함수
    private void AllUpdate()
    {
        StringDataTextUpdate(string.Format("(Player{0})", m_playerId), Texts.PlayerText);
        StringDataTextUpdate(m_result, Texts.ResultText);
        IntDataTextUpdate(m_score, Texts.ScoreText);
        FloatDataTextUpdate(m_gameStartTime, Texts.PlayTimeText);
        IntDataTextUpdate(m_killCount, Texts.KillCountText);
        IntDataTextUpdate(m_multiKillCount, Texts.MultiKillText);
        IntDataTextUpdate(m_hitCount, Texts.HitCountText);
        IntDataTextUpdate(m_totalScore, Texts.TotalScoreText);
    }

    // IntData를 사용하는 UI의 텍스트를 업데이트하는 함수
    private void IntDataTextUpdate(int _intData, Texts _type)
    {
        if (!m_isReady) 
        {
            return;
        }

        // intData를 사용하는 UI가 맞는지 비트플래그 체크
        if (((int)_type & m_intCheckBitFlag) != (int)_type) 
        {
            Debug.LogError("맞지않는 형식의 Data Text로 접근을 시도함");
            return;
        }

        Get<Text>((int)_type).text = string.Format("{0:D5}", _intData);
    }

    // StringData를 사용하는 UI의 텍스트를 업데이트하는 함수
    private void StringDataTextUpdate(string _stringData, Texts _type)
    {
        if (!m_isReady)
        {
            return;
        }

        // stringData를 사용하는 UI가 맞는지 비트플래그 체크
        if (((int)_type & m_strCheckBitFlag) != (int)_type)
        {
            Debug.LogError("맞지않는 형식의 Data Text로 접근을 시도함");
            return;
        }

        Get<Text>((int)_type).text = _stringData;
    }

    // float를 사용하는 UI의 텍스트를 업데이트하는 함수
    private void FloatDataTextUpdate(float _floatData,Texts _type)
    {
        if (!m_isReady)
        {
            return;
        }

        // floatData를 사용하는 UI가 맞는지 비트플래그 체크
        if (((int)_type & m_floatCheckBitFlag) != (int)_type)
        {
            Debug.LogError("맞지않는 형식의 Data Text로 접근을 시도함");
            return;
        }

        if(_type == Texts.PlayTimeText)
        {
            Managers.Game.EndPlayTime = Time.time;
            int l_subTime = (int)(Managers.Game.EndPlayTime - _floatData);
            int l_minute = l_subTime / 60;
            int l_second = l_subTime % 60;

            Get<Text>((int)Texts.PlayTimeText).text = string.Format("{0:D2}:{1:D2}", l_minute, l_second);
        }
    }
}
