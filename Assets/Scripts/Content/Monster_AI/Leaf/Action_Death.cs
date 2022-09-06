using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_Death : BT_Action
{
    private float m_corpseTime = 0.0f;
    MonsterStatTable m_stat = null;

    const string AnimationDeath = "Death";

    public Action_Death(GameObject _object, float _corpseTime)
    {
        m_object = _object.GetComponent<Interface_Enemy>();
        m_animator = m_object.GetComponent<MyAnimator>();
        m_corpseTime = _corpseTime;

        m_stat = m_object.Stat;
    }

    protected override AI.State Function()
    {
        if (m_stat.hp <= 0)
        {
            // ���� State�� Death���� Ȯ�ο�����, if���� ����
            if (m_animator.GetCurrentStateInfo().IsName(AnimationDeath) == false)
            {
                m_animator.FlagClear();
                m_animator.SetBool(AI.Enemy_AniParametar.DeathFlag, true);
                m_object.GetComponent<Rigidbody>().isKinematic = true;
                m_object.GetComponent<Collider>().isTrigger = true;
            }

            Managers.Resource.Destroy(m_object.gameObject, m_corpseTime);

            //m_corpseTime -= Time.deltaTime;
            //if (m_corpseTime < 0) {

            //}

            return AI.State.SUCCESS;
        }

        return AI.State.RUNNING;
    }  
}
