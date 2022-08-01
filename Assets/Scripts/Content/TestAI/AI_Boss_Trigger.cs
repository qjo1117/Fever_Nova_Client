using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Boss_Trigger : MonoBehaviour
{
    private AI_Enemy_Boss m_boss = null;

    public void SetBoss(AI_Enemy_Boss _boss)
    {
        m_boss = _boss;
    }

    void Start()
    {

    }

    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)Define.Layer.Player)
        {
            return;
        }

        if (m_boss != null)
        {
            m_boss.StartBossAI();
        }

        Destroy(gameObject);
    }
}
