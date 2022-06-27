using System.Collections.Generic;

public class Sequence : SkillNode {
    public Sequence() : base() { }
    public Sequence(List<SkillNode> p_children) : base(p_children) { }

    public override SkillNodeState Evaluate()
    {
        bool anyChildIsRunning = false;

        foreach (SkillNode node in m_listChildren) {
            switch (node.Evaluate()) {
                case SkillNodeState.FAILURE:                 // 실패했다면 failure를 준다.
                    m_state = SkillNodeState.FAILURE;
                    return m_state;
                case SkillNodeState.SUCCESS:                 // 실행 가능하면 넘긴다.
                    continue;
                case SkillNodeState.RUNNING:                 // 실행중이면 어떤 노드는 실행중이다라고 체크하고 진행
                    anyChildIsRunning = true;
                    continue;
                default:                                // 예외경우면 성공
                    m_state = SkillNodeState.SUCCESS;
                    return m_state;
            }
        }

        // 만약 실행중이라면 실행중이라고 상태를 변경한다.
        m_state = anyChildIsRunning ? SkillNodeState.RUNNING : SkillNodeState.SUCCESS;
        return m_state;
    }

}