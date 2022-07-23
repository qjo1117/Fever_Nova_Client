using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : UI_Scene
{
    #region UI컴포넌트_ENUM
    enum Texts
    {
        CurrentScoreText,                           // 누적 점수 표시 텍스트
        ScoreLogText                                // 획득 점수 표시 텍스트
    }

    enum Images
    {
        Background                                  // 배경화면
    }
    #endregion

    #region 변수
    private bool            m_timerIsRun = false;   // coreLogTimer 코루틴 함수가 실행중인가?
    private List<int>       m_killScoreList;        // 획득한 점수 리스트

    public float            m_deleteDelay = 5.0f;   // 킬로그 삭제 지연시간
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        m_killScoreList = new List<int>();
    }

    // 현재 점수 UI 상태 갱신하는 함수
    private void ScoreTextUpdate()
    {
        Get<Text>((int)Texts.CurrentScoreText).text = $"점수 : {Managers.Game.Player.MainPlayer.Stat.score}";
    }

    // 획득 점수 텍스트 생성하는 함수
    public void ScoreLogCreate(int _score)
    {
        // 획득 점수 리스트에 점수 추가
        m_killScoreList.Add(_score);

        
        // 코루틴 함수 실행 안된경우, 함수 실행해서 0.5초동안 점수 획득을 대기한후, 0.5초가 지나면
        // 입력받은 획득 점수의 갯수를 확인하여 획득 점수 텍스트를 생성할수 있도록함
        if (!m_timerIsRun)
        {
            StartCoroutine(ScoreLogTimer());
        }
    }

    // 0.5초안에 입력된 점수들 체크하여 Kill 또는 MultiKill 로그를 실질적으로 생성해주는 함수
    IEnumerator ScoreLogTimer()
    {
        // 0.5초동안 대기
        m_timerIsRun = true;
        yield return new WaitForSeconds(0.5f);

        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        PlayerController l_player = Managers.Game.Player.MainPlayer;

        // 획득한 점수가 1개일때 (싱글 킬일떄)
        if(m_killScoreList.Count <= 1)
        {
            if (l_logText.text.Length == 0)
            {
                l_logText.text = $"Kill + {m_killScoreList[0]}\n";
            }
            else
            {
                l_logText.text = l_logText.text.Insert(l_logText.text.Length, $"Kill + {m_killScoreList[0]}\n");
            }

            // 플레이어 Score Stat 업데이트
            l_player.Stat.score += m_killScoreList[0];
        }
        // 획득한 점수가 1개 이상일떄 (멀티 킬일때)
        else
        {
            int l_scoreSum = 0;

            // 총 획득 점수 합산
            foreach (int item in m_killScoreList) 
            {
                l_scoreSum += item;
            }

            if (l_logText.text.Length == 0)
            {
                l_logText.text = $"MultiKill + {l_scoreSum}\n";
            }
            else
            {
                l_logText.text = l_logText.text.Insert(l_logText.text.Length, $"MultiKill + {l_scoreSum}\n");
            }

            // 플레이어 Score Stat 업데이트
            l_player.Stat.score += l_scoreSum;
        }

        ScoreTextUpdate();
        // 추가한 텍스트 일정시간 뒤에 삭제될수있도록 Delete역할 맡는 코루틴함수 시작
        StartCoroutine(ScoreLogDelete());
        m_killScoreList.Clear();
        m_timerIsRun = false;
    }

    // 획득 점수 텍스트 삭제하는 코루틴 함수
    IEnumerator ScoreLogDelete()
    {
        // 설정된 지연시간 만큼 대기
        yield return new WaitForSeconds(m_deleteDelay);

        // 맨 윗줄의 텍스트를 삭제
        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        int l_emptyCharIndex = l_logText.text.IndexOf('\n');
        l_logText.text = l_logText.text.Remove(0, l_emptyCharIndex + 1);
    }

}
