using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy_Boss : AI_Enemy
{
    public AI_Boss_Trigger m_trigger;
    public float Chase_MoveSpeed;
    private bool IsPlayerEnterBossArea;

    public void StartBossAI()
    {
        IsPlayerEnterBossArea = true;
    }

    public bool GetTrigger()
    {
        return IsPlayerEnterBossArea;
    }

    private void Start()
    {
        CreateBehaviorTreeAIState();
    }

    protected override void CreateBehaviorTreeAIState()
    {
        m_trigger.SetBoss(this);
        m_enemyType = AI.EnemyType.Melee;
        m_brain = new BT_Root();

        BT_Selector l_mainSelector = new BT_Selector();

        BT_Sequence l_combatSQ = new BT_Sequence();

        AI_Combat_Trigger l_trigger
            = new AI_Combat_Trigger(gameObject);

        AI_Combat_Chase l_chase
            = new AI_Combat_Chase(gameObject, Chase_MoveSpeed);

        l_combatSQ.AddChild(l_trigger);
        l_combatSQ.AddChild(l_chase);


        //BT_Sequence l_idleSQ = new BT_Sequence();

        //l_idleSQ.AddChild(l_idleSQ);

        l_mainSelector.AddChild(l_combatSQ);
        //l_mainSelector.AddChild(l_idleSQ);

        m_brain.Child = l_mainSelector;
    }
}
