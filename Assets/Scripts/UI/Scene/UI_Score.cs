using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : UI_Scene
{
    enum Texts
    {
        CurrentScoreText,
        ScoreLogText
    }

    enum Images
    {
        Background
    }

    private bool            m_timerIsRun = false;
    private List<int>       m_killScoreList;

    public float            m_deleteDelay = 5.0f;

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        m_killScoreList = new List<int>();
    }

    public void SetScoreText()
    {
        Get<Text>((int)Texts.CurrentScoreText).text = $"Á¡¼ö : {Managers.Game.Player.MainPlayer.Stat.score}";
    }

    public void ScoreLogCreate(int _score)
    {
        m_killScoreList.Add(_score);

        if (!m_timerIsRun)
        {
            StartCoroutine(ScoreLogTimer());
        }
    }

    IEnumerator ScoreLogTimer()
    {
        m_timerIsRun = true;
        yield return new WaitForSeconds(0.5f);

        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        PlayerController l_player = Managers.Game.Player.MainPlayer;

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

            l_player.Stat.score += m_killScoreList[0];
        }
        else
        {
            int l_scoreSum = 0;

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

            l_player.Stat.score += l_scoreSum;
        }

        SetScoreText();
        StartCoroutine(ScoreLogDelete());
        m_killScoreList.Clear();
        m_timerIsRun = false;
    }

    IEnumerator ScoreLogDelete()
    {
        yield return new WaitForSeconds(m_deleteDelay);

        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        int l_emptyCharIndex = l_logText.text.IndexOf('\n');
        l_logText.text = l_logText.text.Remove(0, l_emptyCharIndex + 1);
    }

}
