using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Condition_IsDeath : BT_Condition
{
    private GameObject m_object;
    private MonsterStatTable m_stat;
    private bool m_isPlayingAnimation;

    public Condition_IsDeath(GameObject _object, MonsterStatTable _Stat)
    {
        m_object = _object;
        m_stat = _Stat;
        m_isPlayingAnimation = false;
    }

    public override AI.State Update()
    {
        return IsDeath();
    }

    private AI.State IsDeath()
    {
        if (m_stat.HP > 0)
        {
            return AI.State.FAILURE;
        }

        return AI.State.SUCCESS;
    }
}