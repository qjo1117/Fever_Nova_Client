using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : MonoBehaviour
{

    private int                 m_id = -1;
    private int                 m_attack = 30;
    private int                 m_hp = 100;

    // 아마 추가 될꺼같은거
    private int m_playerId = -1;

    public int ID { get => m_id; set => m_id = value; }
    public int Attack { get => m_attack; }
    public int Hp { get => m_hp; set => m_hp = value; }

    // Start is called before the first frame update
    void Start()
    {
    }

    public void PlayerAttack()
	{
        Debug.Log("때림");
	}

    // Update is called once per frame
    void Update()
    {
    }



}
