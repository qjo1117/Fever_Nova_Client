using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_WorldSpaceHPBar
{
    public override void Init()
    {
        base.Init();
    }

    protected override void Update()
    {
        // 부모클래스의 Update 꼭 !호출해주어야함
        // 체력의바의 월드 위치 업데이트 함수가 부모 Update함수에 존재하기 떄문
        base.Update();

        // 몬스터 hp바 테스트용
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if (m_hp > 0)
            {
                HP = m_hp - 10;
            }

        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (m_hp < m_maxHp)
            {
                HP = m_hp + 10;
            }
        }
    }
}
