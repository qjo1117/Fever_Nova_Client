using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TaskAttack : SkillNode 
{
    private Transform m_transform = null;
    private SkillTree m_skillTree = null;
    private PlayerController m_player = null;

    private float m_attackTime = 1.0f;
    private float m_attackCount = 0.0f;
    private float m_range = 0.0f;

    public TaskAttack(Transform p_transform)
    {
        m_transform = p_transform;
        m_skillTree = p_transform.GetComponent<SkillTree>();
        m_range = (float)m_skillTree.GetData("CheckRange");
    }

    public override SkillNodeState Evaluate()
    {
        // 타겟 여부
        m_player = (PlayerController)GetData("Target");
        if(m_player == null) {
            return SkillNodeState.FAILURE;
		}

        // 시간 체크
        m_attackCount += Time.deltaTime;
        if (m_attackTime >= m_attackCount) {
            return SkillNodeState.SUCCESS;
		} 
        m_attackCount = 0.0f;

        // 거리 체크
        Vector3 dist = m_player.transform.position - m_transform.position;
        if (dist.sqrMagnitude <= m_range * m_range) {
            m_player.Demege(10);            // 일단 Manager통해서 말고 직접적으로 때리게 만듬
            Debug.Log("때림");
        }

        m_state = SkillNodeState.SUCCESS;
        return m_state;
    }

}