using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_AI:MonoBehaviour
{
    //����(���濹��)
    public uint hp;
    public float skill_cooltime;
    public float attack_range;
    public float detect_range;
    public float alarm_range;
    public float MoveSpeed;
    //AI�� ���� �� ��Ʈ
    protected BT_Root m_brain;
    //���� Ÿ��
    [SerializeField]
    protected AI.EnemyType enemy_type;
    //���� ���� ����
    [SerializeField]
    protected AI.EnemyState enemy_state;

    //Ʈ�� �����
    protected virtual void CreateTree()
    {

    }

    public void Awake()
    {
        //�ӽ������� �ʱ�ȭ
        m_brain = new BT_Root();
        enemy_type = AI.EnemyType.Melee;
        enemy_state = AI.EnemyState.Recon;
    }
    public void Update()
    {
        //Ʈ���� ƽ���� ������.
        m_brain.Tick();
    }
}
