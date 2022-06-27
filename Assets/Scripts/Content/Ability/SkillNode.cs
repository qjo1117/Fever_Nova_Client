using System.Collections;
using System.Collections.Generic;

public enum SkillNodeState {
    RUNNING,
    SUCCESS,
    FAILURE
}

public class SkillNode {
    protected SkillNodeState m_state = SkillNodeState.FAILURE;          // 생각보다 쓸모없음

    protected SkillNode m_parent = null;                                // 부모 노드를 알려준다.
    protected List<SkillNode> m_listChildren = new List<SkillNode>();   // 자식 노드를 알려준다.

    private Dictionary<string, object> m_dicDataContext = new Dictionary<string, object>();     // 예제에 있던 데이터 맵핑용도인데 SkillTree쪽에서 사용해도된다.

    public SkillNode()
    {
        m_parent = null;
    }

    // 스킬 노드리스트를 받으면 연결을 해준다.
    public SkillNode(List<SkillNode> children)
    {
        foreach (SkillNode child in children) {
            Attach(child);
        }
    }

    // 연결해준다.
    private void Attach(SkillNode p_node)
    {
        p_node.m_parent = this;
        m_listChildren.Add(p_node);
    }

    // 기본 상태를 넣으면 실패로 돌린다.
    public virtual SkillNodeState Evaluate()
	{
        return SkillNodeState.FAILURE;
	}

    // 데이터를 넣습니다.
    public void SetData(string p_type, object p_value)
    {
        if(m_dicDataContext.ContainsKey(p_type) == true) {
            m_dicDataContext[p_type] = p_value;
        }
        else {
            m_dicDataContext.Add(p_type, p_value);
		}
    }

    // 데이터를 가져옵니다.
    public object GetData(string p_type)
    {
        object value = null;
        if (m_dicDataContext.TryGetValue(p_type, out value)) {
            return value;
        }

        // 데이터를 순회해서 데이터가 있는지 체크한다.
        SkillNode node = m_parent;
        while (node != null) {
            value = node.GetData(p_type);
            if (value != null)
                return value;
            node = node.m_parent;
        }
        return null;
    }

    // 기존에 가지고 있는 데이터를 삭제합니다.
    public bool ClearData(string p_type)
    {
        if (m_dicDataContext.ContainsKey(p_type)) {
            m_dicDataContext.Remove(p_type);
            return true;
        }

        SkillNode node = m_parent;
        while (node != null) {
            // 노드들의 데이터를 삭제합니다.
            bool cleared = node.ClearData(p_type);
            if (cleared)
                return true;
            node = node.m_parent;
        }
        return false;
    }
}

