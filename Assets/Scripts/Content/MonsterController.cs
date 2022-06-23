using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{

    private int m_id = -1;
    private int m_attack = 30;

    private int m_hp = 100;

    public int ID { get => m_id; set => m_id = value; }
    public int Attack { get => m_attack; }
    public int Hp { get => m_hp; set => m_hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
