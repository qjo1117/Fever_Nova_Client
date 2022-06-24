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

    private AbilitySystem       m_abilitySystem = null;
    private MonsterSkill        m_currentSkill = MonsterSkill.None;

    // 원거리, 근거리
    public enum MonsterSkill {
        None,
        Attack,

	}

    public int ID { get => m_id; set => m_id = value; }
    public int Attack { get => m_attack; }
    public int Hp { get => m_hp; set => m_hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_abilitySystem = GetComponent<AbilitySystem>();

        m_abilitySystem.AddSkill(MonsterSkill.Attack.ToString(), 1.0f, PlayerAttack, "Explosion_FX");
    }

    public void PlayerAttack()
	{
        Debug.Log("때림");
	}

    // Update is called once per frame
    void Update()
    {
        foreach(Ability ability in m_abilitySystem.Ability) {
            if(ability.IsAction == true) {
                ability.Action();
			}
		}
    }


    private void CheckToSelectSkill()
	{
        if(m_currentSkill != MonsterSkill.None) {
            return;
		}

        List<int> listSkill = new List<int>();
        List<Ability> abilitys = m_abilitySystem.Ability;
        int size = abilitys.Count;

        // 스킬 발동 가능 여부를 체크한다.
        for (int i = 0; i < size; ++i) {
            if(abilitys[i].IsAction == true) {
                listSkill.Add(i);
            }
        }

        // 현재 가능한 인덱스에서 스킬 범위를 검출한다.
        size = listSkill.Count;
        for (int i = 0; i < size; ++i) {
            // Skill일 경우
            if((abilitys[i] as Skill) != null) {
                Skill skill = (Skill)abilitys[i];

                // 여기서 부터는 보여주기식
                // PlayerController target = Manager.Game.Player[m_playerId];
                // Vector3 dist = transform.position - target.transform.position;
                // if(dist.magnitude > skill.DetectedRange) {           // 감지범위안에 잇을 경우
                //      // 공격을 실행하든 또 인덱스 빼와서 확률 적용하든 하자
                // }
			}
        }
	}
}
