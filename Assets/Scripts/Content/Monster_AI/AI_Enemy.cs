using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField]
    public MonsterStat m_stat;
    public UI_MonsterHPBar m_hpBar;
    //��ų����Ʈ �߰��صѰ�
    //�ּ�ó���� ���� = ��ų�� ����ȭ�� ������ ����...
    //AI ���� ���� ����
    //  ����� Detect�� �����ϸ� ������ �÷��̾� ��ġ���� ����
    //  �̰� Detect ���� �� AI�� ���� skill �� ���� Ȥ�� �ൿ��Ŀ� ���� ��ų�� ���� ��
    //  ������ ��ų�� �����Ÿ������� ���ٽ�ų ����
    //  ������ ��...��ų�� ����ȭ�� ������ ���� �ؼ� �ٷ� chase�� �Ѿ�� �س���
    //  AI_Enemy_01,AI_Enemy_Boss ��ũ��Ʈ�� �ش���� ���뿹��
    //[SerializeField]
    //public List<Skill> m_skills;
    protected BT_Root m_brain;
    protected AI.EnemyType m_enemyType;

    private Rigidbody m_rigid = null;


    public void Init()
    {
        CreateBehaviorTreeAIState();
        m_rigid = GetComponent<Rigidbody>();
    }

    public virtual void AddPatrolPoint(Vector3 _position) { }

    protected virtual void CreateBehaviorTreeAIState() { }

    protected void Update()
    {
        m_brain.Tick();
    }

    // ���ظ� �Ծ�����
    // Vector�� ���� Defualt���ǰ� �ȵǹǷ� �Լ��� �ΰ��� ������ �۵���Ų ��

    public void Demege(int _attack)
	{
        m_stat.Hp -= _attack;
        m_hpBar.HP = m_stat.Hp;
        // Die
        if (m_stat.Hp <= 0) {

		}
    }

    public void Demege(int _attack, Vector3 _force)
	{
        m_rigid.AddForce(_force);
        Demege(_attack);
    }
}