using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DeathCheck : BT_Action
{
    private MonsterStatTable m_stat;
    private bool m_isPlayingAnimation;

    public AI_DeathCheck(GameObject _object, MonsterStatTable _Stat)
    {
        m_object = _object;
        m_stat = _Stat;
        m_isPlayingAnimation = false;
    }

    public override AI.State Update()
    {
        return DeathCheck();
    }

    private AI.State DeathCheck()
    {
        if (m_stat.HP > 0)
        {
            return AI.State.FAILURE;
        }

        // ��� �۾� ó��
        // ������Ʈ ���� �� �ִϸ��̼� ���� ��
        if (!m_isPlayingAnimation)
        {
            m_isPlayingAnimation = true;
            m_animator.CrossFade("Death", 0.15f);
        }
        return AI.State.SUCCESS;
    }
}
