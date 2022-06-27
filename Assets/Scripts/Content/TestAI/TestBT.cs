using System.Collections.Generic;
using UnityEngine;


public class TestBT : SkillTree 
{
    // 인스펙터에서 값을 지정해준다. 이유 : 기획쪽에서 조절하겠다고 한다.
    [SerializeField]
    private Vector3[] m_wayPoints;
    [SerializeField]
    private float m_range = 5.0f;

    protected override SkillNode SetupTree()
    {
        // 데이터 미리 셋팅
        SetData("MoveSpeed", (float)5.0f);
        SetData("CheckRange", m_range);

        // Sequence : 전부 순회해서 실행
        // Selector : 한녀석만 실행
        SkillNode root = new Selector(new List<SkillNode>
        {
            new Sequence(new List<SkillNode> {
                new TestCheckEnemy(transform),              // 위치 체크후
                new TestTargetToRun(transform),             // 추격 (공격 준비)
                new TaskAttack(transform),                  // 공격
            }),
            new TaskPatrol(transform, m_wayPoints),

		});
        
        return root;
    }


	private void OnDrawGizmos()
    { 
        DrawWayPoint();
        DrawCheckRange();

    }

    private void DrawCheckRange()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_range);
	}

    private void DrawWayPoint()
	{
        Gizmos.color = Color.red;

        if (m_wayPoints.Length == 0) {
            return;
        }
        int size = m_wayPoints.Length;
        for (int i = 0; i < size - 1; ++i) {
            Gizmos.DrawLine(m_wayPoints[i], m_wayPoints[i + 1]);
            Gizmos.DrawSphere(m_wayPoints[i], 0.5f);
        }
        Gizmos.DrawSphere(m_wayPoints[size - 1], 0.5f);
    }
}