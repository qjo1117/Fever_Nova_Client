using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 HP바의 경우 다른 플레이어의 HP바를 출력시킬수도 있으므로
// Main Player와 연동시킬 것이 아니라 연동할 PlayerController를 받아와야함.

public class UI_PlayerHPBar : UI_Scene
{
    #region UI컴포넌트_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp바 뒷배경 (hp바 하얀색 배경)
        hpBar                                                    // hp바       (녹색 hp바)
    }

    protected enum GameObjects
    {
        hpLine                                                  // 하얀색 사각형프레임(Image,Image(1)....) 관리 오브젝트 
    }
    #endregion

    #region 변수
    private PlayerController m_target = null;
    private int              m_hp = 0;                          // hp바 출력에 사용될 hp값

    public int m_unitHp;                                        // 단위 hp 
                                                                // (단위 hp가 낮을수록 최대 체력이 증가했을떄 하얀색 사각형이 촘촘하개 배열됨)
    #endregion

    #region 프로퍼티
    public PlayerController Target { get => m_target; set => m_target = value; }

    // 프로퍼티를 통해 hp값을 변경하면 자동으로 hpBar의 상태를 갱신하도록함
    public int HP
    {
        get 
        { 
            return m_hp; 
        }
        set
        {
            m_hp = value;
            HpBarUpdate();
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();
        HpBarUpdate();
    }

    private void Update()
    {
        // 플레이어의 최대 체력증가 (test용)
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Managers.Game.Player.MainPlayer.Stat.maxHp += 100;
            Debug.Log($"current maxhp : {m_target.Stat.maxHp}");
            GetHpBoost();
        }
    }

    public void GetHpBoost()
    {

        // (단위 hp / 최대체력) 계산하여 하얀색 사각 프레임의 스케일값 구함
        float l_scaleX = (float)m_unitHp / m_target.Stat.maxHp;

        // hpLine 오브젝트의 자식들 (하얀색 사각 프레임들)의 스케일값을 변경
        // (HorizontalLayoutGroup 컴포넌트가 활성화 되어있으면 Scale이 제대로 적용이 안되므로 적용전에 잠시 비활성화 시켜줌)

        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }


    // hp 바의 상태 갱신하는 함수
    private void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.hp / (float)m_target.Stat.maxHp;
    }
}
