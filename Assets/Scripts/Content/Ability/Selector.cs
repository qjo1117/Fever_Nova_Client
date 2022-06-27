using System.Collections.Generic;

public class Selector : SkillNode {
    public Selector() : base()  { }

    public Selector(List<SkillNode> p_children) : base(p_children) { }

    public override SkillNodeState Evaluate()
    {
        // 노드를 순회해서 값을 확인합니다.
        foreach (SkillNode node in m_listChildren) {
            switch (node.Evaluate()) {
                case SkillNodeState.FAILURE:            // 실패할 경우 다음 경우를 확인
                    continue;
                case SkillNodeState.SUCCESS:            // 성공할 경우 성공을 리턴
                    m_state = SkillNodeState.SUCCESS;
                    return m_state;
                case SkillNodeState.RUNNING:            // 실행중이면 실행중이라고 알려준다.
                    m_state = SkillNodeState.RUNNING;
                    return m_state;
                default:                                // 이상한 경우면 넘긴다.
                    continue;
            }
        }

        // 계속 실패했으면 실패라고 알려준다.
        m_state = SkillNodeState.FAILURE;
        return m_state;
    }

}

