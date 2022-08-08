using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkillList
{
    melee,
    range,
    selfdestroy,
}

public class SkillManager : MonoBehaviour
{
    private Dictionary<SkillList, BT_Action> m_skills;

    public void Init()
    {
        m_skills = new Dictionary<SkillList, BT_Action>();

    }

    public object Skills(SkillList _index) => m_skills[_index].Copy();
}