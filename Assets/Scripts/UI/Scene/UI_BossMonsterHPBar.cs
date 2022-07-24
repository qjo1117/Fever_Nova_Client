using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BossMonsterHPBar : UI_SceneHPBar
{
    #region 변수
    [SerializeField]
    private Camera              m_mainCamera;
    #endregion

    public override void Init()
    {
        base.Init();

        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // 보스 몬스터 hp바 테스트용
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            if(m_hp > 0)
            {
                HP = m_hp - 10;
            }
            
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if(m_hp < m_maxHp)
            {
                HP = m_hp + 10;
            }
        }
    }
}
