using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Boom : MonoBehaviour
{
    [SerializeField]
    private float               m_moveSpeed = 400.0f;               // 폭탄의 가해지는 힘
    [SerializeField]
    private float               m_explosionRange = 5.0f;            // 폭발 반경
    [SerializeField]
    private float               m_detectRange = 2.0f;               // 감지 반경

    // 이것만 필요할꺼같아서 어빌리티는 안씀
    private float               m_explosionDelayTime = 0.0f;        // 현재 지연 시간
    [SerializeField]
    private float               m_explosionMaxDelayTime = 5.0f;     // 최대 지연 시간
    private bool                m_isExplosion = false;

    private PlayerController    m_player = null;
    private Rigidbody           m_rigid = null;

    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    private bool                m_isDelayState = false;

    public void Create(Vector3 p_dir, PlayerController p_player)
	{
        gameObject.SetActive(true);
        m_player = p_player;
        transform.position = p_player.transform.position + p_dir;
        m_explosionDelayTime = 0.0f;
        m_isExplosion = false;
        m_rigid.AddForce(p_dir * m_moveSpeed);
    }

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        DaleyCheck();
        Explosion();

        
    }

    private void DaleyCheck()
	{
        m_explosionDelayTime += Time.deltaTime;

        if(m_explosionMaxDelayTime > m_explosionDelayTime) {
            return;
		}

        m_isDelayState = true;
    }

    
    // TODO : 갈아엎자.
    private void Explosion()
	{

    }


	private void OnDrawGizmos()
	{

    }
}
