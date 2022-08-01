using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField]
    public MonsterStat m_stat;
    public UI_MonsterHPBar m_hpBar;
    //스킬리스트 추가해둘것
    //주석처리한 이유 = 스킬이 정형화된 문서가 없어...
    //AI 변경 예정 사항
    //  현재는 Detect가 성공하면 무작정 플레이어 위치까지 따라감
    //  이걸 Detect 성공 시 AI가 가진 skill 중 랜덤 혹은 행동양식에 따른 스킬을 선정 후
    //  선정된 스킬의 사정거리까지만 접근시킬 생각
    //  지금은 뭐...스킬이 정형화된 문서도 없고 해서 바로 chase로 넘어가게 해놨음
    //  AI_Enemy_01,AI_Enemy_Boss 스크립트에 해당사항 적용예정
    //[SerializeField]
    //public List<Skill> m_skills;
    protected BT_Root m_brain;
    protected AI.EnemyType m_enemyType;

    public void Init()
    {
        CreateBehaviorTreeAIState();
    }

    public virtual void AddPatrolPoint(Vector3 _position) { }

    protected virtual void CreateBehaviorTreeAIState() { }

    protected void Update()
    {
        m_brain.Tick();
    }
}
