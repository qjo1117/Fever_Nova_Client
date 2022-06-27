using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTree : MonoBehaviour
{
    private Dictionary<string, object>  m_dicData = new Dictionary<string, object>();
    private BehaviorNode                m_root = null;

    protected void Start()
    {
        m_root = SetupTree();
    }

    protected void Update()
    {
        if (m_root != null) {
            m_root.Update();
        }
    }

    // 추상 클래스
    protected abstract BehaviorNode SetupTree();

    public void SetData(string p_key, object p_data)
	{
        // 만약 사전에 등록되어있으면 그 값을 설정한다.
        if (m_dicData.ContainsKey(p_key) == true){
            m_dicData[p_key] = p_data;
        }
        // 등록 안되었다면 추가
        else {
            m_dicData.Add(p_key, p_data);
        }
    }

    public object GetData(string p_key)
	{
        // 사전에 등록되어 있으면 반환
        object data = null;
        if(m_dicData.TryGetValue(p_key, out data) == true) {
            return data;
        }

        // 아니면 Null반환
        return null;
	}

    public T GetData<T>(string p_key)
    {
        return (T)GetData(p_key);
    }

}
