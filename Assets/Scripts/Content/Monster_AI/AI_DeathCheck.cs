using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_DeathCheck : BT_Action
{
    private GameObject m_object;
    private MonsterStatTable m_stat;

    public AI_DeathCheck(GameObject _object, MonsterStatTable _Stat)
    {
        m_object = _object;
        m_stat = _Stat;
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

        // 사망 작업 처리
        // 오브젝트 삭제 및 애니메이션 제거 등
        return AI.State.SUCCESS;
    }
}
