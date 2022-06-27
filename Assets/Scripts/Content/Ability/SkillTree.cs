using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillTree : MonoBehaviour 
{
    // 트리의 루트 노드를 알아야한다.
    private SkillNode m_root = null;

    // 현재 사용할 변수를 보여준다.
    // 정의한 이유 : 나중에 애니메이션처럼 사용하기 위해서
    [SerializeField]
    protected Dictionary<string, object> m_dicDataContext = new Dictionary<string, object>();

    public void SetData(string p_type, object p_data)
	{
        // 만약 기존에 데이터가 있을 경우
        if(m_dicDataContext.ContainsKey(p_type) == true) {
            m_dicDataContext[p_type] = p_data;
        }
        // 만약 기존에 데이터가 없을경우 추가를 해준다.
        else {
            m_dicDataContext.Add(p_type, p_data);
		}
	}

    public object GetData(string p_type)
	{
        object value = null;
        m_dicDataContext.TryGetValue(p_type, out value);
        return value;
    }

    protected void Start()
    {
        m_root = SetupTree();
    }

    private void Update()
    {
        if (m_root != null) {
            m_root.Evaluate();
        }
    }

    protected abstract SkillNode SetupTree();

}
