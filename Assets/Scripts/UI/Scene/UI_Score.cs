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

    public float            m_deleteDelay = 5.0f;


    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));
    }

    public void CurrentScoreUpdate(int _score)
    {
        Managers.Game.Player.MainPlayer.Stat.score += _score;

        Get<Text>((int)Texts.CurrentScoreText).text = $"Á¡¼ö : {Managers.Game.Player.MainPlayer.Stat.score}";
    }

    public void ScoreLogCreate(int _score)
    {
        Text l_logText = Get<Text>((int)Texts.ScoreLogText);

        if (l_logText.text.Length == 0)
        {
            l_logText.text = $"Kill + {_score}\n";
        }
        else
        {
            l_logText.text = l_logText.text.Insert(l_logText.text.Length, $"Kill + {_score}\n");
        }

        StartCoroutine(ScoreLogDelete());
    }

    IEnumerator ScoreLogDelete()
    {
        yield return new WaitForSeconds(m_deleteDelay);

        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        int l_emptyCharIndex = l_logText.text.IndexOf('\n');
        l_logText.text = l_logText.text.Remove(0, l_emptyCharIndex + 1);
    }

}
